
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSeverMain.Model
{

    /// <summary>
    /// 角色的数据模型
    /// </summary>
    public class UserModel
    {
        public int Id;
        public string Name;
        public int Been;
        public int WinCount;
        public int LoseCount;
        public int RunCount;
        public int Lv;
        public int Exp;

        /// <summary>
        /// 与这个角色所关联的账号ID
        /// </summary>
        public int AccountId;

        public UserModel(int id,string name,int accountId)
        {
            this.Id = id;
            this.Name = name;
            this.Been = 10000;
            this.WinCount = 0;
            this.LoseCount = 0;
            this.RunCount = 0;
            this.Lv = 0;
            this.Exp = 0;
            this.AccountId = accountId;
        }
    }
}
