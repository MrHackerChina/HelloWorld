using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSever;
using Protocol;
using Protocol.Dto;
using GameSeverMain.Cache;
using Protocol.Code;

namespace GameSeverMain.Login
{
    /// <summary>
    /// 账号处理的逻辑层
    /// </summary>
    public class AccountHandler : IHandler
    {
        AccountCache accountCache = Caches.Account;
        public void OnDisconnent(ClientPeer client)
        {
            if (accountCache.IsOnline(client))
                accountCache.Offline(client);
        }

        public void OnReceive(ClientPeer client, int subCode, object value)
        {
            switch (subCode)
            {
                case AccountCode.LOGIN:
                    AccountDto dto = value as AccountDto;
                    login(client, dto.Account, dto.Password);
                    break;
                case AccountCode.REGIST_CREQ:
                    AccountDto dt = value as AccountDto;
                    regist(client, dt.Account, dt.Password);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public void regist(ClientPeer client, string account, string password)
        {
            SingleExecute.Instance.Excute(() =>
            {
                if (accountCache.IsExist(account))
                {
                    //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "账号异存在");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -1);
                    return;
                }
                if (string.IsNullOrEmpty(account))
                {
                    //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "账号输入不合法");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -2);

                    return;
                }

                if (string.IsNullOrEmpty(password) || password.Length < 4 || password.Length > 16)
                {
                    //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "密码不合法");
                    client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, -3);

                    return;
                }
                accountCache.Create(account, password);
                //client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, "注册成功");
                client.Send(OpCode.ACCOUNT, AccountCode.REGIST_SRES, 0);

            });

        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        private void login(ClientPeer client, string account, string password)
        {
            SingleExecute.Instance.Excute(() =>
            {
                if (!accountCache.IsExist(account))
                {
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号不存在");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -1);

                    return;
                }
                if (!accountCache.IsMatch(account, password))
                {
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号 & 密码 错误");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -2);

                    return;
                }
                if (accountCache.IsOnline(account) || accountCache.IsOnline(client))
                {
                    //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "账号 & 密码 异地登录");
                    client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, -3);

                    return;
                }
                accountCache.Online(account, client);
                //client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, "登录成功");
                client.Send(OpCode.ACCOUNT, AccountCode.LOGIN, 0);

            });

        }
    }
}
