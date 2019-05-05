using GameSever;
using GameSever.Util.Concurrent;
using GameSeverMain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSeverMain.Cache
{
    /// <summary>
    /// 账号缓存
    /// </summary>
    public class AccountCache
    {
        /// <summary>
        /// 账号 对应是数据模型
        /// </summary>
        private Dictionary<string, AccountModel> accModelDict = new Dictionary<string, AccountModel>();

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsExist(string account)
        {
            return accModelDict.ContainsKey(account);
        }
        /// <summary>
        /// 账号 用来存储ID
        /// </summary>
        private ConcurrentInt id = new ConcurrentInt(-1);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public void Create(string account,string password)
        {
            AccountModel model = new AccountModel(id.Add_Get(),account, password);
            accModelDict.Add(model.Account, model);
        }

        /// <summary>
        /// 获取账号模型
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public AccountModel GetModel(string account)
        {
            return accModelDict[account];
        }

        /// <summary>
        /// 验证账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsMatch(string account,string password)
        {
            AccountModel model = accModelDict[account];
            return model.Password == password;
        }

        /// <summary>
        /// 账号是对应连接对象
        /// </summary>
        private Dictionary<string, ClientPeer> accClientDict = new Dictionary<string, ClientPeer>();
        private Dictionary<ClientPeer, string> clientAccDict = new Dictionary<ClientPeer, string>();


        /// <summary>
        /// 账号单独在线 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool IsOnline(string account)
        {
            return accClientDict.ContainsKey(account);
        }
        /// <summary>
        /// 是否有重复的连接对象
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool IsOnline(ClientPeer client)
        {
            return clientAccDict.ContainsKey(client);
        }
        /// <summary>
        /// 用户上线
        /// </summary>
        /// <param name="account"></param>
        /// <param name="client"></param>
        public void Online(string account,ClientPeer client)
        {
            accClientDict.Add(account, client);
            clientAccDict.Add(client, account);
        }
        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(ClientPeer client)
        {
            string account = clientAccDict[client];
            clientAccDict.Remove(client);
            accClientDict.Remove(account);
        }
        public void Offline(string account)
        {
            ClientPeer client = accClientDict[account];
            clientAccDict.Remove(client);
            accClientDict.Remove(account);
        }

        /// <summary>
        /// 获取在线玩家的ID
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int GetId(ClientPeer client)
        {
            string account = clientAccDict[client];
            AccountModel model = accModelDict[account];
            return model.Id;
        }
    }
}
