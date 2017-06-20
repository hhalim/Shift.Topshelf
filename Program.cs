using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Shift.Topshelf
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appServiceName = ConfigurationManager.AppSettings["ServiceName"];

            HostFactory.Run(x =>                                 
            {
                x.Service<ShiftServerService>(
                    s =>                        
                    {
                        s.ConstructUsing(name => new ShiftServerService());     
                        s.WhenStarted(async tc => await tc.StartAsync());              
                        s.WhenStopped(tc => tc.Stop());               
                    }
                );

                x.StartManually();
                x.RunAsLocalSystem();                            

                x.SetDescription(appServiceName);        
                x.SetDisplayName(appServiceName);                       
                x.SetServiceName(appServiceName);                       
            });
        }
    }
}
