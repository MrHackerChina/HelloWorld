using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace GameSever
{
    public class ServerPeer
    {
        /// <summary>
        /// 服务器的socket
        /// </summary>
        private Socket severSocekt = null;
        /// <summary>
        /// 限制客户端连接数量的信号
        /// </summary>
        private Semaphore acceptSemaphore;

        /// <summary>
        /// 客户端连接池
        /// </summary>
        private ClientPeerPool clientPeerPool;

        private IApplication appliction;

        public void SetApplication(IApplication app)
        {
            this.appliction = app;
        }
        /// <summary>
        /// 开启服务器
        /// </summary>
        /// <param name="port"></param>
        /// <param name="maxCount"></param>
        public void Start(int port, int maxCount)
        {
            try
            {
                severSocekt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                acceptSemaphore = new Semaphore(maxCount, maxCount);

                clientPeerPool = new ClientPeerPool(maxCount);
                ClientPeer tempClientPeer = null;
                for (int i = 0; i < maxCount; i++)
                {
                    tempClientPeer = new ClientPeer();
                    tempClientPeer.ReceiveArgs.Completed += receive_Completed;
                    tempClientPeer.receiveCompleted = receiveCompleted;
                    tempClientPeer.sendDisconnect = Disconnect;
                    clientPeerPool.Enqueue(tempClientPeer);
                }

                severSocekt.Bind(new IPEndPoint((IPAddress.Any), port));

                severSocekt.Listen(10);
                Console.WriteLine("服务器启动");
                startAccpet(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

      
        #region 接受客户端的连接
        /// <summary>
        /// 开始等待客户端的连接
        /// </summary>
        /// <param name="e"></param>
        private void startAccpet(SocketAsyncEventArgs e)
        {
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += accept_Completed;
            }
          
            //e.Completed += accept_Completed;
            // 判断异步事件是否执行完毕
            bool result = severSocekt.AcceptAsync(e);
            if (!result)
            {
                proccessAccept(e);
            }
        }

        /// <summary>
        /// 接受连接请求异步事件完成时候触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accept_Completed(object sender, SocketAsyncEventArgs e)
        {
            proccessAccept(e);
        }



        /// <summary>
        /// 处理连接请求
        /// </summary>
        /// <param name="e"></param>
        private void proccessAccept(SocketAsyncEventArgs e)
        {
            //限制线程的访问  计数
            acceptSemaphore.WaitOne();

            // 得到客户端的对象
            //Socket clientSocket = e.AcceptSocket;
            ClientPeer client = clientPeerPool.Depueue();
            client.ClientSocket = e.AcceptSocket;

            //appliction.OnConnent(client);
            Console.WriteLine("KHD: success " + client.ClientSocket.RemoteEndPoint.ToString());

            // 保存处理 
            startReceive(client);

            //递归调用
            e.AcceptSocket = null;
            startAccpet(e);

        }

        #endregion


        #region 接受数据
        /// <summary>
        /// 开始接受数据
        /// </summary>
        /// <param name="client"></param>
        private void startReceive(ClientPeer client)
        {
            try
            {
                bool result = client.ClientSocket.ReceiveAsync(client.ReceiveArgs);
                if (!result)
                {
                    processReceive(client.ReceiveArgs);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 处理接受的数据
        /// </summary>
        /// <param name="receiveArgs"></param>
        private void processReceive(SocketAsyncEventArgs e)
        {
            ClientPeer client = e.UserToken as ClientPeer;
            //判断网络消息是否接受成功
            if (client.ReceiveArgs.SocketError == SocketError.Success && client.ReceiveArgs.BytesTransferred > 0)
            {
                byte[] byteArray = new byte[client.ReceiveArgs.BytesTransferred];
                Buffer.BlockCopy(client.ReceiveArgs.Buffer, 0, byteArray, 0, client.ReceiveArgs.BytesTransferred);
                client.startReceive(byteArray);

                startReceive(client);
            }
            //断开连接  如果没有传输的数据
            else if (client.ReceiveArgs.BytesTransferred == 0)
            {
                // 客户端主动断开连接
                if(client.ReceiveArgs.SocketError== SocketError.Success)
                {
                    Disconnect(client, "客户端主动断开连接");
                }
                // 由于网络异常 导致 被动断开连接
                else 
                {
                    Disconnect(client, client.ReceiveArgs.SocketError.ToString());
                }


            }
        }

        /// <summary>
        /// 当接受完成时 触发是事件
        /// </summary>
        /// <param name="send"></param>
        /// <param name="e"></param>
        private void receive_Completed(object sender, SocketAsyncEventArgs e)
        {
            processReceive(e);
        }


        /// <summary>
        /// 一条数据解析完成的处理
        /// </summary>
        /// <param name="client"></param>
        /// <param name="value"></param>
        private void receiveCompleted(ClientPeer client, SocketMsg msg)
        {
            //给应用层 让其使用
            appliction.OnReceive(client, msg);
        }


        #endregion



        #region 断开连接

        public void Disconnect(ClientPeer client,string reason)
        {
            try
            {
                if(client!=null)
                {
                    Console.WriteLine(client.ClientSocket.RemoteEndPoint.ToString() + "KHD fail " + reason);
                    appliction.OnDisconnent(client);
                    client.Disconnect();
                    //回收
                    clientPeerPool.Enqueue(client);
                    //限制线程的访问  计数
                    acceptSemaphore.Release();
                }
                else
                {
                    throw new Exception("当前客户端为空");
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion


        #region

        #endregion


        #region 发送数据


         
        #endregion
    }
}
