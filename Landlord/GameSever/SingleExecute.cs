using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameSever
{


    public delegate void ExceuteDelegate();
    /// <summary>
    /// 单线程池
    /// </summary>
    public class SingleExecute
    {

        private static SingleExecute instance = null;

        private static object obj = new object();
        public static SingleExecute Instance
        {
            get
            {
                lock(obj)
                {
                    if (instance == null)
                        instance = new SingleExecute();
                    return instance;
                }
               
            }
        }

        /// <summary>
        /// 互斥锁
        /// </summary>
        public Mutex mutex;

        private SingleExecute()
        {
            mutex = new Mutex();
        }

        /// <summary>
        /// 单线程处理逻辑
        /// </summary>
        /// <param name="exceuteDelegate"></param>
        public void Excute(ExceuteDelegate exceuteDelegate)
        {
            lock (this)
            {
                mutex.WaitOne();
                exceuteDelegate();
                mutex.ReleaseMutex();
            }
        }
    }
}
