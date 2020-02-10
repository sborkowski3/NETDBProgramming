using System;
using System.IO;
using NLog;
using System.Collections.Generic;
using System.Linq;


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

            if (!File.Exists(file))
            {
                loggerError("File does not exist {File}", file);
            }
            else
            {
                string choice;
                {
                    Console.WriteLine("1) Add your favorite Movie");
                    Console.WriteLine("2) Show all Movies from list");
                    Console.WriteLine("3) Press any button to exit");

                    choice = Console.ReadLine();
                    logger.Info("User choice: {Choice}", choice);

                    List<UInt64> MovieIds = new List<UInt64>();
                    List<string> MovieTitles = new List<string>();
                    List<string> MovieGenres = new List<string>();

                    try
                    {
                        StreamReader sr = new StreamReader(file);
                        sr.ReadLine();
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            int idx = line.IndexOf('"');
                            if (idx == -1)
                            {
                                string[] movieDetails = line.Split(',');
                                MovieIds.Add(UInt64.Parse(movieDetails[0]));
                                MovieGenres.Add(movieDetails[2].Replace("|", ","));
                            }
                            else
                            {
                                MovieIds.Add(UInt64.Parse(line.Substring(0, idx - 1)));
                                line = line.Substring(idx + 1);
                                idx = line.IndexOf('"');
                                MovieTitles.Add(line.Substring(0, idx));
                                line = line.Substring(idx + 2);
                                MovieGenres.Add(line.Replace("|", ","));
                            }
                        }
                        sr.Close();

                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                    }
                    logger.Info("Movies in file {Count}", MovieIds.Count);

                    if (choice == "1")
                    {
                        Console.WriteLine("Enter the movie title: ");
                        string MovieTitle = Console.ReadLine();
                        List<string> LowerCaseMovieTitles = MovieTitles.ConvertAll(t => t.ToLower());
                        if (LowerCaseMovieTitles.Contains(MovieTitle.ToLower()))
                        {
                            Console.WriteLine("That movie has already been entered");
                            logger.Info("Duplicate movie title {Title}", movieTitle);
                        }
                        else
                        {
                            UInt64 movieId = MovieIds.Max() + 1;
                            List<string> genres = new List<string>();
                            string genre;
                        }
                        do
                        {
                            Console.WriteLine("Enter genre (or done to quit)");
                            genre = Console.ReadLine();
                            if (genre != "done" && genre.Length > 0)
                            {
                                genres.Add(genre);
                            }
                        } while (genre != "done");
                        if (genre.Count == 0)
                        {
                            genres.Add("(no genres listed)");
                        }
                        string genreString = string.Join("|", genres);
                        movieTitle = movieTitle.IndexOf(',') != -1 ? $"\"{MovieTitle}\"" : MovieTitle;
                        StreamWriter sw = new StreamWriter(file, true);
                        sw.WriteLine($"{movieId},{movieTitle},{genreString}");
                        sw.Close();
                        MovieIds.Add(movieId);
                        MovieTitles.Add(genreString);
                        logger.info("Movie id {Id} added", movieId);

                    }
                }
                else if (choice == "2")
                {
                    for (int i = 0; i < MovieIds.Count; i++)
                    {
                        Console.WriteLine($"Id: {MovieIds[i]}");
                        Console.WriteLine($"Title: {MovieTitles[i]}");
                        Console.WriteLine($"Genre(s): {MovieGenres[i]}");
                        Console.WriteLine();
                    }
                }
            
            while (choice == "1" || choice == "2") ;

        }
        logger.Info("Program ended");
       }
}
    }

