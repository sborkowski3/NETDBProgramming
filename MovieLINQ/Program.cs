using System;
using NLog;

namespace MovieLINQ
{
    class MainClass
    {
        // create a class level instance of logger (can be used in methods other than Main)
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            logger.Info("Program started");

            string scrubbedFile = FileScrubber.ScrubMovies("../../movies.csv");
            MovieFile movieFile = new MovieFile(scrubbedFile);

            logger.Info("Program ended");
        }
    }
}
