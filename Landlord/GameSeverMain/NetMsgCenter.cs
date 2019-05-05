using GameSever;
using GameSeverMain.Login;
using Protocol.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSeverMain
{
    /// <summary>
    /// 网络消息中心
    /// </summary>
    public class NetMsgCenter : IApplication
    {
        private IHandler accout = new AccountHandler();
        private IHandler user = new UserHandler();
        private MatchHandler match = new MatchHandler();
        private ChatHandler chat = new ChatHandler();
        private FightHandler fight = new FightHandler();

        public NetMsgCenter()
        {
            match.startFight += fight.startFight;
        }


        public void OnConnent(ClientPeer client)
        {

        }

        public void OnDisconnent(ClientPeer client)
        {
            fight.OnDisconnent(client);
            chat.OnDisconnent(client);
            match.OnDisconnent(client);
            user.OnDisconnent(client);
            accout.OnDisconnent(client);
        }

        public void OnReceive(ClientPeer client, SocketMsg msg)
        {
            
            switch (msg.OpCode)
            {
                case OpCode.ACCOUNT:
                    accout.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.USER:
                    user.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.MATCH:
                    match.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.CHAT:
                    chat.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                case OpCode.FIGHT:
                    fight.OnReceive(client, msg.SubCode, msg.Value);
                    break;
                default:
                    break;
            }
        }
    }
}
