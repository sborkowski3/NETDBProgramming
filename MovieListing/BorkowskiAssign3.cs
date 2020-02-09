using System;
using System.IO;
using NLog;

namespace MovieData
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //Call Nlog
            var config = new NLog.Config.LoggingConfiguration();

            //Define targets
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "movies.csv" };
            var logconsole = new NLog.Targets.Console.Target("logconsole");

            //specify min log level to max log level and target
            config.AddRule(Log.Level.Trace, LogLevel.Fatal, logconsole);
            config.AddRule(Log.Level.Info, LogLevel.Fatal, logfile);

            //apply NLog config
            NLog.LogManager.Configuration = config;

            //create instance of LogManager
            var logger = N.LogLogManager.GetCurrentClassLogger();
            logger.Info("Program started");

            //make a menu, ask for input
            Console.WriteLine("Enter 1 to create data file");
            Console.WriteLine("Enter 2 to parse data");
            Console.WriteLine("Enter anything else to quit");
            //input response
            string resp = Console.ReadLine();

            //specify path for data file
            string file = "users/sborkowski3.downloads/movies.csv";

        }
    }
}
