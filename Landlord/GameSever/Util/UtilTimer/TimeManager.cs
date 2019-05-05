using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Collections.Concurrent;
using GameSever.Util.Concurrent;

namespace GameSever.Util.UtilTimer
{
    /// <summary>
    ///  定时器 管理类
    /// </summary>
    public class TimeManager
    {
        private static TimeManager instance = null;

        public static TimeManager Instance
        {
            get
            {
                lock (instance)
                {
                    if (instance == null)
                        instance = new TimeManager();
                    return instance;
                }

            }
        }
        /// <summary>
        ///  任务ID  任务模型
        /// </summary>
        private ConcurrentDictionary<int, TimerModel> idModelDict = new ConcurrentDictionary<int, TimerModel>();

        /// <summary>
        /// 移除的ID 列表
        /// </summary>
        private List<int> removeList = new List<int>();

        /// <summary>
        /// 线程安全的int
        /// </summary>
        private ConcurrentInt id = new ConcurrentInt(-1);


        Timer timer;
        public TimeManager()
        {
            timer = new Timer(10);
            timer.Elapsed += Time_Elapsed;
        }

        /// <summary>
        /// 到达时间触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Time_Elapsed(object sender, ElapsedEventArgs e)
        {

            lock (removeList)
            {
                TimerModel tempModel = null;
                foreach (var id in removeList)
                {
                    idModelDict.TryRemove(id, out tempModel);
                }
                removeList.Clear();
            }


            foreach (var model in idModelDict.Values)
            {
                if (model.Time <= DateTime.Now.Ticks)
                    model.Run();
            }
        }

        /// <summary>
        /// 添加定时任务
        /// </summary>
        /// <param name="dataTime"></param>
        /// <param name="timerDelegate"></param>
        public void AddTimerEvent(DateTime dataTime, TimerDelegate timerDelegate)
        {
            long delayTime = dataTime.Ticks - DateTime.Now.Ticks;
            if (delayTime <= 0)
                return;
            AddTimerEvent(delayTime, timerDelegate);
        }

        public void AddTimerEvent(long delayTime, TimerDelegate timerDelegate)
        {
            TimerModel model = new TimerModel(id.Add_Get(), DateTime.Now.Ticks + delayTime, timerDelegate);
            idModelDict.TryAdd(model.Id, model);
        }
    }
}
