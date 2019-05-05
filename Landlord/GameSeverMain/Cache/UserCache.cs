using GameSeverMain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameSever.Util.Concurrent;
using GameSever;

namespace GameSeverMain.Cache
{
    /// <summary>
    /// 角色数据缓存层
    /// </summary>
    public class UserCache
    {
        /// <summary>
        /// 角色的id  数据
        /// </summary>
        private Dictionary<int, UserModel> idModelDict = new Dictionary<int, UserModel>();

        /// <summary>
        /// 账号ID  对应的角色ID
        /// </summary>
        private Dictionary<int, int> accIdUIdDict = new Dictionary<int, int>();

        ConcurrentInt id = new ConcurrentInt(-1);

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        /// <param name="accountId"></param>
        public void Create(ClientPeer client, string name,int accountId)
        {
            UserModel model = new UserModel(id.Add_Get(), name, accountId);
            idModelDict.Add(model.Id, model);
            accIdUIdDict.Add(model.AccountId, model.Id);
        }


        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public bool IsExist(int accountId)
        {
            return accIdUIdDict.ContainsKey(accountId);
        }

        public UserModel GetModelByAccountID(int accountId)
        {
            int userId = accIdUIdDict[accountId];
            UserModel model = idModelDict[userId];
            return model;
        }

        public int Get_Id(int account)
        {
            return accIdUIdDict[account];
        }

        /// <summary>
        /// 存储在线玩家
        /// </summary>
        private Dictionary<int, ClientPeer> idClientDict = new Dictionary<int, ClientPeer>();

        private Dictionary<ClientPeer, int> clientIdDict = new Dictionary<ClientPeer, int>();

        public bool IsOnline(ClientPeer client)
        {
            return clientIdDict.ContainsKey(client);
        }
        public bool IsOnline(int id)
        {
            return idClientDict.ContainsKey(id);
        }

        public void Update(UserModel model)
        {
            idModelDict[model.Id] = model;
        }

        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="id"></param>
        public void Online(ClientPeer client,int id)
        {
            clientIdDict.Add(client, id);
            idClientDict.Add(id, client);
        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(ClientPeer client)
        {
            int id = clientIdDict[client];
            clientIdDict.Remove(client);
            idClientDict.Remove(id);
        }

        /// <summary>
        /// 根据连接对象获取角色模型
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public UserModel GetModelByClientPeer(ClientPeer client)
        {
            int id = clientIdDict[client];
            UserModel model = idModelDict[id];
            return model;
        }

        public UserModel GetModelById(int userId)
        {
            UserModel model = idModelDict[userId];
            return model;
        }

        /// <summary>
        /// 根据角色id获取连接对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClientPeer GetClientPeer(int id)
        {
            return idClientDict[id];
        }

        /// <summary>
        /// 根据玩家连接对象获取ID
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int Get_Id(ClientPeer client)
        {
            return clientIdDict[client];
        }
    }
}
