using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;
//安装服务 F:\Test\QuartzConsole\TopSelf\bin\Debug\TopSelf.exe install
//卸载服务 F:\Test\QuartzConsole\TopSelf\bin\Debug\TopSelf.exe uninstall
//启动服务 F:\Test\QuartzConsole\TopSelf\bin\Debug\TopSelf.exe start
namespace TopSelf
{
    public class Kiba1: ServiceControl
    {
        readonly Timer _timer;
        public Kiba1()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => Console.WriteLine($"{DateTime.Now} Kiba1 running");
        }
    
        public bool Start(HostControl hostControl)
        {
            _timer.Start();
            return true;
        }
         
        public bool Stop(HostControl hostControl)
        {
            _timer.Stop();
            return true;
        }
    }
    public class Kiba2 : ServiceControl
    {
        readonly Timer _timer;
        public Kiba2()
        {
            _timer = new Timer(2000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => Console.WriteLine($"{DateTime.Now} Kiba2 running");
        }

        public bool Start(HostControl hostControl)
        {
            _timer.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _timer.Stop();
            return true;
        }
    }

    public class KibaCustom
    {
        readonly Timer _timer;
        public KibaCustom()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => Console.WriteLine($"{DateTime.Now} KibaCustom running");
        }
        public void Start() { _timer.Start(); }
        public void Stop() { _timer.Stop(); }
    }
    public class Program
    {
        public static void Main()                                
        {
            //新建服务 new
            HostFactory.New(x =>                                 
            {                                                    
                x.Service<Kiba1>();    //使用继承了ServiceControl的Kiba1类                                       
                x.RunAsLocalSystem();      //服务使用NETWORK_SERVICE内置帐户运行。身份标识，有好几种方式，如：x.RunAs("username", "password");  x.RunAsPrompt(); x.RunAsNetworkService(); 等                      

                x.SetDescription("服务描述");           
                x.SetDisplayName("服务显示名");                  
                x.SetServiceName("服务名");                      
            }).Run();
            HostFactory.New(x =>
            {
                x.Service<Kiba2>();    //使用继承了ServiceControl的Kiba2类                                       
                x.RunAsLocalSystem();      //服务使用NETWORK_SERVICE内置帐户运行。身份标识，有好几种方式，如：x.RunAs("username", "password");  x.RunAsPrompt(); x.RunAsNetworkService(); 等                      

                x.SetDescription("服务描述2");
                x.SetDisplayName("服务显示名2");
                x.SetServiceName("服务名2");
            }).Run();
           
            //运行服务 Run
            HostFactory.Run(x =>
            {
                x.Service<KibaCustom>(s =>
                {
                    s.ConstructUsing(name => new KibaCustom()); //使用Kiba1类进行构建服务
                    s.WhenStarted(tc => tc.Start());       //使用Kiba1的Start作为启动函数
                    s.WhenStopped(tc => tc.Stop());        //使用Kiba1的Stop作为停止函数
                });
                x.RunAsLocalSystem();      //服务使用NETWORK_SERVICE内置帐户运行。身份标识，有好几种方式，如：x.RunAs("username", "password");  x.RunAsPrompt(); x.RunAsNetworkService(); 等                      

                x.SetDescription("服务描述");
                x.SetDisplayName("服务显示名");
                x.SetServiceName("服务名");
            });
        }


    }
}
