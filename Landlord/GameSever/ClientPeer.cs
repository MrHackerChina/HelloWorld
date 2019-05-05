using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace GameSever
{
    /// <summary>
    /// 封装客户端
    /// </summary>
    public class ClientPeer
    {
        public Socket ClientSocket { get; set; }

        public ClientPeer()
        {
            this.ReceiveArgs = new SocketAsyncEventArgs();
            this.ReceiveArgs.UserToken = this;
            this.ReceiveArgs.SetBuffer(new byte[1024], 0, 1024);
            this.SendArg = new SocketAsyncEventArgs();
            this.sendQueue = new Queue<byte[]>();
            this.SendArg.Completed += SendArg_Completed;

        }

      

        public delegate void ReceiveCompleted(ClientPeer client, SocketMsg msg);
        /// <summary>
        /// 一个消息解析完成的回调
        /// </summary>
        public ReceiveCompleted receiveCompleted;
        /// <summary>
        /// 接受的数据
        /// </summary>
        private List<byte> dataCache = new List<byte>();

        public SocketAsyncEventArgs ReceiveArgs { get; set; }

        /// <summary>
        /// 是否正在处理数据
        /// </summary>
        private bool IsReceiveProcess = false;
         
        /// <summary>
        /// 自身处理数据包
        /// </summary>
        /// <param name="data"></param>
        public void startReceive(byte[] packet)
        {
            dataCache.AddRange(packet);
            if (!IsReceiveProcess)
                processReceive();

        }

        private void processReceive()
        {
            IsReceiveProcess = true;
            byte[] data = EncodeTool.DecodePacket(ref dataCache);
            if(data==null)
            {
                IsReceiveProcess = false;
                return;
            }
            //
            SocketMsg msg = EncodeTool.DecodeMsg(data);
            //回调给上层
            receiveCompleted?.Invoke(this, msg);
            //if (receiveCompleted != null)
            //    receiveCompleted(this, value);
            processReceive();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            //清空数据
            dataCache.Clear();
            IsReceiveProcess = false;
            sendQueue.Clear();
            isSendProcess = false;

            //关闭Socket
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            ClientSocket = null;

        }


        #region  发送数据

        /// <summary>
        /// 发送的消息队列
        /// </summary>
        private Queue<byte[]> sendQueue;

        private bool isSendProcess = false;

        /// <summary>
        /// 发送的异步套接字操作
        /// </summary>
        private SocketAsyncEventArgs SendArg;

        public delegate void SendDisconnect(ClientPeer client, string reason);
        public SendDisconnect sendDisconnect;
        /// <summary>
        /// 发送网络消息
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="subCode"></param>
        /// <param name="value"></param>
        public void Send(int opCode, int subCode, object value)
        {
            SocketMsg msg = new SocketMsg(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMeg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);
            Send(packet);
        }
        public void Send(byte[] packet)
        {
            sendQueue.Enqueue(packet);
            if (!isSendProcess)
                send();
        }

        

        private void send()
        {
            isSendProcess = true;
            if(sendQueue.Count==0)
            {
                isSendProcess = false;
                return;
            }
            byte[] packet = sendQueue.Dequeue();
            SendArg.SetBuffer(packet, 0, packet.Length);
            bool result = ClientSocket.SendAsync(SendArg);
            if(!result)
            {
                processSend();
            }
        }

        private void SendArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            processSend();
        }

        private void processSend()
        {
            if (SendArg.SocketError != SocketError.Success)
            {
                // 出错 客户端断开连接
                sendDisconnect(this, SendArg.SocketError.ToString());

            }
            else
            {
                send();
            }
        }
        #endregion

        /*
                /// 粘包和拆包问题  解决策略 
                /// 消息头 int 长度
                /// 和消息尾 data
                void test()
                {
                    byte[] byt = Encoding.Default.GetBytes("12345");
                    int length = byt.Length;
                    byte[] by = BitConverter.GetBytes(length);

                }

            */

    }
}
