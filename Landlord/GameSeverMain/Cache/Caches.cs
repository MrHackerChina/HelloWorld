using GameSeverMain.Cache.Fight;
using GameSeverMain.Cache.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSeverMain.Cache
{
    /// <summary>
    /// 账号唯一性
    /// </summary>
    public static class Caches
    {

        public static AccountCache Account { get; set; }
        public static UserCache User { get; set; }
        public static MatchCache Match { get; set; }
        public static FightCache Fight { get; set; }
        static Caches()
        {
            Account = new AccountCache();
            User = new UserCache();
            Match = new MatchCache();
            Fight = new FightCache();
        }
    }
}
