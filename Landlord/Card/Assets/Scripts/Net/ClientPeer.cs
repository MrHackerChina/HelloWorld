using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;

public class ClientPeer
{

    public  Socket socket;
    private string ip;
    private int port;
    //public bool isConnect = false;

    public ClientPeer(string ip, int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ip = ip;
            this.port = port;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Connent()
    {
        try
        {
            socket.Connect(ip, port);
            Debug.Log("连接服务器成功");
            startReceive();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    #region 接受数据
    /// <summary>
    /// 接受的数据缓冲区
    /// </summary>
    private byte[] receivBuffer = new byte[1024];

    /// <summary>
    /// 接受的数据
    /// </summary>
    private List<byte> dataCache = new List<byte>();

    private bool isProcessReceive = false;

    public Queue<SocketMsg> socketMsgList = new Queue<SocketMsg>();

    private void startReceive()
    {
        if (socket == null && socket.Connected == false)
        {
            Debug.LogError("没有连接成功服务器");
            return;
        }
        socket.BeginReceive(receivBuffer, 0, 1024, SocketFlags.None, receiveCallBack, socket);
    }

    private void receiveCallBack(IAsyncResult ar)
    {
        try
        {
            int length = socket.EndReceive(ar);
            byte[] tempByteArray = new byte[length];
            Buffer.BlockCopy(receivBuffer, 0, tempByteArray, 0, length);
            dataCache.AddRange(tempByteArray);
            //处理收到的数据
            if (!isProcessReceive)
                processReceive();

            startReceive();
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            Debug.LogError(e.Message);
        }
    }


    private void processReceive()
    {
        isProcessReceive = true;
        byte[] data = EncodeTool.DecodePacket(ref dataCache);
        if (data == null)
        {
            isProcessReceive = false;
            return;
        }

        SocketMsg msg = EncodeTool.DecodeMsg(data);
        socketMsgList.Enqueue(msg);

        //Debug.Log(msg.Value);

        processReceive();
    }
    #endregion

    #region 发送数据
    public void Send(int opCode, int subCode, object value)
    {
        SocketMsg msg = new SocketMsg(opCode, subCode, value);
        Send(msg);
    }

    public void Send(SocketMsg msg)
    {
        byte[] data = EncodeTool.EncodeMeg(msg);
        byte[] packet = EncodeTool.EncodePacket(data);
        try
        {
            //socket.BeginSend(packet);
            socket.Send(packet);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    #endregion


    public void Dispose()
    {
        try
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Disconnect(true);
            socket.Close();
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

}
