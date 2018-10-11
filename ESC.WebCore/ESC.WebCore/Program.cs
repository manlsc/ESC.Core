using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ESC.WebCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.UseKestrel((options) =>
            //{
            //    options.Limits.MaxConcurrentConnections = 100;  //客户端最大连接数
            //    options.Limits.MaxConcurrentUpgradedConnections = 100;
            //    options.Limits.MaxRequestBodySize = 10 * 1024;  //请求正文最大大小
            //    options.Limits.MinRequestBodyDataRate =  
            //        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));  //请求正文最小数据速率
            //    options.Limits.MinResponseDataRate =
            //        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
            //    options.Listen(IPAddress.Loopback, 5000);
            //})
                .UseStartup<Startup>()
                .Build();
    }
}
