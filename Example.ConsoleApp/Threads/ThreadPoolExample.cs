using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Example.ConsoleApp
{
    public class ThreadPoolExample
    {
        public void Launch()
        {
            int workerThreads, completionPortThreads;

            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine("线程池中辅助线程的最大数目：{0}.线程池中异步 I/O 线程的最大数目：{1}", workerThreads, completionPortThreads);

            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine("线程池根据需要创建的最少数量的辅助线程：{0}.线程池根据需要创建的最少数量的异步 I/O 线程：{1}", workerThreads, completionPortThreads);

            //设置线程池默认参数
            ThreadPool.SetMaxThreads(100, 100);
            ThreadPool.SetMinThreads(2, 2);

            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine("线程池中辅助线程的最大数目：{0}.线程池中异步 I/O 线程的最大数目：{1}", workerThreads, completionPortThreads);

            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine("线程池根据需要创建的最少数量的辅助线程：{0}.线程池根据需要创建的最少数量的异步 I/O 线程：{1}", workerThreads, completionPortThreads);

            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
            Console.WriteLine("可用辅助线程的数目：{0}.可用异步 I/O 线程的数目：{1}", workerThreads, completionPortThreads);
        }
    }
}
