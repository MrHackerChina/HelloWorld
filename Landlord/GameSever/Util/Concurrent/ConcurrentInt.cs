﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSever.Util.Concurrent
{
    /// <summary>
    /// 线程安全的int类型
    /// </summary>
    public class ConcurrentInt
    {

        private int value;

        public ConcurrentInt(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public int Add_Get()
        {
            lock(this)
            {
                value++;
                return value;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public int Reduce_Get()
        {
            lock (this)
            {
                value--;
                return value;
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public int Get()
        {
            return value;
        }
    }
}
