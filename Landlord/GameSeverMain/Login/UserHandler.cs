using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSever;
using Protocol.Code;
using GameSeverMain.Cache;
using GameSeverMain.Model;
using Protocol.Dto;

namespace GameSeverMain.Login
{
    /// <summary>
    /// 用户逻辑处理层
    /// </summary>
    public class UserHandler : IHandler
    {

        private UserCache userCache = Caches.User;

        private AccountCache accountCache = Caches.Account;
        public void OnDisconnent(ClientPeer client)
        {
            if (userCache.IsOnline(client))
                userCache.Offline(client);
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case UserCode.CREATE_CREQ:
                    create(client, value.ToString());
                    break;
                case UserCode.GET_INFO_CREQ:
                    getInfo(client);
                    break;
                case UserCode.ONLINE_CREQ:
                    online(client);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        private void create(ClientPeer client, string name)
        {
            SingleExecute.Instance.Excute(() =>
            {
                //
                if (!accountCache.IsOnline(client))
                {
                    client.Send(OpCode.USER, UserCode.CREATE_SERS, -1);
                    return;
                }
                int account = accountCache.GetId(client);
                if (userCache.IsExist(account))
                {
                    client.Send(OpCode.USER, UserCode.CREATE_SERS, -2);
                    return;
                }
                userCache.Create(client, name, account);
                client.Send(OpCode.USER, UserCode.CREATE_SERS, 0);
            });

        }

        /// <summary>
        /// 获取角色对象
        /// </summary>
        /// <param name="client"></param>
        private void getInfo(ClientPeer client)
        {
            SingleExecute.Instance.Excute(() =>
            {
                if (!accountCache.IsOnline(client))
                {
                    //client.Send(OpCode.USER, UserCode.GET_INFO_SRES, -1);
                    return;
                }
                int account = accountCache.GetId(client);
                if (!userCache.IsExist(account))
                {
                    client.Send(OpCode.USER, UserCode.GET_INFO_SRES, null);
                    return;
                }
                //有角色 上线  防止2次上线
                if (!userCache.IsOnline(client))
                    online(client);

                UserModel model = userCache.GetModelByAccountID(account);
                UserDto dto = new UserDto(model.Id,model.Name, model.Been, model.WinCount, model.LoseCount, model.RunCount, model.Lv, model.Exp);
                client.Send(OpCode.USER, UserCode.GET_INFO_SRES, dto);
            });

        }


        private void online(ClientPeer client)
        {
            SingleExecute.Instance.Excute(() =>
            {
                if (!accountCache.IsOnline(client))
                {
                    client.Send(OpCode.USER, UserCode.ONLINE_SERS, -1);
                    return;
                }
                int account = accountCache.GetId(client);
                if (!userCache.IsExist(account))
                {
                    client.Send(OpCode.USER, UserCode.GET_INFO_SRES, -2);//没有角色
                    return;
                }
                int userId = userCache.Get_Id(account);

                userCache.Online(client, userId);
                client.Send(OpCode.USER, UserCode.ONLINE_SERS, 0);
            });

        }
    }


}
