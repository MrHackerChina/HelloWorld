using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
    public  class Identity
    {
        /// <summary>
        /// 农民
        /// </summary>
        public const int FARMER = 0;

        /// <summary>
        /// 地主
        /// </summary>
        public const int LANDLORD = 1;


        public static string GetString(int identity)
        {
            string result = "";
            if (identity == 0)
                result = "农民";
            else
                result = "地主";
            return result;
        }
            
    }
}
