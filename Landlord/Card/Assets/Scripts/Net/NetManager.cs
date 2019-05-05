using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class NetManager : ManagerBase
{
    public static NetManager Instance = null;

    #region 处理客户端内部给服务器发消息的事件
    private void Awake()
    {
        Instance = this;
        Add(0, this);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case 0:
                client.Send(message as SocketMsg);
                break;
            default:
                break;
        }
    }

    #endregion


    private ClientPeer client = new ClientPeer("127.0.0.1", 9999);
    void Start()
    {
        client.Connent();
    }

    float awaitTime = 5f;
    float newTime = 0f;
    private void Update()
    {
        if (client == null)
            return;
        if (client.socket.Connected == false)
        {
            newTime += Time.deltaTime;
            if (newTime <= awaitTime)
                return;
            //client.socket.Dispose();
            client = new ClientPeer("127.0.0.1", 9999);
            //client.Dispose();
            client.Connent();
            newTime = 0f;
            return;
        }

        while (client.socketMsgList.Count > 0)
        {
            SocketMsg msg = client.socketMsgList.Dequeue();
            proccessSocketMsg(msg);
        }
    }
    #region 处理接受到服务器的消息

    HanderBase accountHandler = new AccoutHander();
    HanderBase userHandler = new UserHandler();
    HanderBase matchHandler = new MatchHandler();
    HanderBase chatHandler = new ChatHandler();
    HanderBase fightHandler = new FightHandler();
    /// <summary>
    /// 接受网络消息
    /// </summary>
    /// <param name="msg"></param>
    private void proccessSocketMsg(SocketMsg msg)
    {
        switch (msg.OpCode)
        {
            case OpCode.ACCOUNT:
                accountHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            case OpCode.USER:
                userHandler.OnReceive(msg.SubCode, msg.Value);
                break; ;
            case OpCode.MATCH:
                matchHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            case OpCode.CHAT:
                chatHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            case OpCode.FIGHT:
                fightHandler.OnReceive(msg.SubCode, msg.Value);
                break;
            default:
                break;
        }
    }

    #endregion
}

