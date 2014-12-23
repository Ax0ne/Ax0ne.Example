using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace Example.Web.Assit
{
    public class WebTimer
    {
        private static readonly Timer _timer = new Timer(OnTimerElapsed);
        private static readonly WorkTaskHost _jobHost = new WorkTaskHost();
        private static bool _isRunTimer = false;
        /// <summary>
        /// 启动定时器
        /// </summary>
        public static void Start()
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromHours(23.999));
            //_timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(60000)); // test uncomment
        }

        private static void OnTimerElapsed(object sender)
        {
            // 如果是应用程序第一次启动,就不运行工作
            if (!_isRunTimer)
            {
                _isRunTimer = true;
                return;
            }
            _jobHost.DoWork(() =>
            {
                // do someting
            });
        }

    }
    /// <summary>
    /// 定时工作宿主类
    /// <para>参考文章:http://haacked.com/archive/2011/10/16/the-dangers-of-implementing-recurring-background-tasks-in-asp-net.aspx
    /// </para>
    /// <para>LastModified:20140918 11:23 By:Ax0ne</para>
    /// </summary>
    public class WorkTaskHost : IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _isShuttingDown;
        public WorkTaskHost()
        {
            // 注册到AppDomain
            HostingEnvironment.RegisterObject(this);
        }
        // 当AppDomain停止时 会调用Stop方法
        public void Stop(bool immediate)
        {
            if (this._isShuttingDown) return;
            lock (_lock)
            {
                if (this._isShuttingDown) return;
                this._isShuttingDown = true;
                HostingEnvironment.UnregisterObject(this);
            }
        }
        // 这个方法会更健壮的运行,IIS的影响对它最小
        public void DoWork(Action action)
        {
            if (this._isShuttingDown) return;
            lock (_lock)
            {
                if (this._isShuttingDown) return;
                action();
            }
        }
    }
}