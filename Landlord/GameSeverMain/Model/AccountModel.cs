using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSeverMain.Model
{
    /// <summary>
    /// 账号的数据模型
    /// </summary>
    public class AccountModel
    {
        public int Id;
        public string Account;
        public string Password;

        public AccountModel(int id,string account,string paw)
        {
            this.Id = id;
            this.Account = account;
            this.Password = paw;
        }
    }
}
