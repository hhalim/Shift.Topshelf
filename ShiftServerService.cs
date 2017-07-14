using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shift.Topshelf
{
    public class ShiftServerService
    {
        private static Shift.JobServer jobServer;

        public ShiftServerService()
        {
            var config = new Shift.ServerConfig();
            config.MaxRunnableJobs = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRunableJobs"]);
            //config.ProcessID = ConfigurationManager.AppSettings["ShiftPID"];
            config.DBConnectionString = ConfigurationManager.ConnectionStrings["ShiftDBConnection"].ConnectionString;
            config.DBAuthKey = ConfigurationManager.AppSettings["DocumentDBAuthKey"];
            config.Workers = Convert.ToInt32(ConfigurationManager.AppSettings["ShiftWorkers"]);

            config.StorageMode = ConfigurationManager.AppSettings["StorageMode"];
            var progressDBInterval = ConfigurationManager.AppSettings["ProgressDBInterval"];
            if (!string.IsNullOrWhiteSpace(progressDBInterval))
                config.ProgressDBInterval = TimeSpan.Parse(progressDBInterval); //Interval when progress is updated in main DB

            var autoDeletePeriod = ConfigurationManager.AppSettings["AutoDeletePeriod"];
            config.AutoDeletePeriod = string.IsNullOrWhiteSpace(autoDeletePeriod) ? null : (int?)Convert.ToInt32(autoDeletePeriod);

            config.AssemblyFolder = ConfigurationManager.AppSettings["AssemblyFolder"];
            //config.AssemblyListPath = ConfigurationManager.AppSettings["AssemblyListPath"];

            config.ForceStopServer = Convert.ToBoolean(ConfigurationManager.AppSettings["ForceStopServer"]); //Set to true to allow windows service to shut down after a set delay in StopServerDelay
            config.StopServerDelay = Convert.ToInt32(ConfigurationManager.AppSettings["StopServerDelay"]);

            config.ServerTimerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]); //optional: default every 5 sec for getting jobs ready to run and run them
            config.ServerTimerInterval2 = Convert.ToInt32(ConfigurationManager.AppSettings["CleanUpTimerInterval"]); //optional: default every 10 sec for server CleanUp()
            //config.AutoDeleteStatus = new List<JobStatus?> { JobStatus.Completed, JobStatus.Error }; //Auto delete only the jobs that had Stopped or with Error
            //config.UseCache = Convert.ToBoolean(ConfigurationManager.AppSettings["UseCache"]);
            //config.CacheConfigurationString = ConfigurationManager.AppSettings["RedisConfiguration"];
            //config.EncryptionKey = ConfigurationManager.AppSettings["ShiftEncryptionParametersKey"];
            config.PollingOnce = Convert.ToBoolean(ConfigurationManager.AppSettings["PollingOnce"]);

            jobServer = new Shift.JobServer(config);
        }

        public async Task StartAsync()
        {
            try
            {
                await jobServer.RunServerAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Stop()
        {
            jobServer.StopServerAsync().GetAwaiter().GetResult(); //Run synchronously or it will exit before marking running jobs with 'STOP' command!!!
        }
    }
}
