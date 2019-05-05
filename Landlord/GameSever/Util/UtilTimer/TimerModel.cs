using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameSever.Util.UtilTimer
{

    /// <summary>
    /// 当定时器达到时间后的触发
    /// </summary>
    public delegate void TimerDelegate();

    /// <summary>
    /// 定时任务
    /// </summary>
    public class TimerModel
    {
        /// <summary>
        /// 任务执行的事件
        /// </summary>
        public long Time;
        public int Id;

        private TimerDelegate timeDelegate;
        public TimerModel(int id ,long time,TimerDelegate td)
        {
            this.Id = id;
            this.Time = time;
            this.timeDelegate = td;
        }

        /// <summary>
        /// 触发任务
        /// </summary>
        public void Run()
        {
            timeDelegate();
        }

    }
}
