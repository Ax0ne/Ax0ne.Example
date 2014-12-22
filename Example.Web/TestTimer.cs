using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Example.Web
{
    public class TestTimer : IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool isShuttingDown;
        public TestTimer()
        {
            HostingEnvironment.RegisterObject(this);
        }
        public void Stop(bool immediate)
        {
            if (isShuttingDown)
                return;
            lock (_lock)
            {
                if (isShuttingDown)
                    return;
                isShuttingDown = true;
                HostingEnvironment.UnregisterObject(this);
            }
        }
        public void DoWork(Action action)
        {
            if (isShuttingDown)
                return;
            lock (_lock)
            {
                if (isShuttingDown)
                    return;
                action();
            }
        }
    }
}