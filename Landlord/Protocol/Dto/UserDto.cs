using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// 用户数据的传输模型
    /// </summary>
    [Serializable]
    public class UserDto
    {
        public int Id;
        public string Name;
        public int Been;
        public int WinCount;
        public int LoseCount;
        public int RunCount;
        public int Lv;
        public int Exp;

        public UserDto()
        {

        }

        public UserDto(int id, string name,int been,int winCount,int loseCount,int runCount,int lv ,int exp)
        {
            this.Id = id;
            this.Name = name;
            this.Been = been;
            this.WinCount = winCount;
            this.LoseCount = loseCount;
            this.RunCount = runCount;
            this.Lv = lv;
            this.Exp = exp;
        }
    }

}
