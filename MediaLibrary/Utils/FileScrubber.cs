using System;
using System.IO;
using System.Linq;
using MediaLibrary.Models.Media;

namespace MediaLibrary.Utils
{
    public static class FileScrubber
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public static string ScrubMovies(string readFile)
        {
            try
            {
                // determine name of writeFile
                string ext = readFile.Split('.').Last();
                string writeFile = readFile.Replace(ext, $"scrubbed.{ext}");
                // if writeFile exists, the file has already been scrubbed
                if (File.Exists(writeFile))
                {
                    // file has already been scrubbed
                    _logger.Info("File already scrubbed");
                }
                else
                {
                    // file has not been scrubbed
                    _logger.Info("File scrub started");
                    // open write file
                    StreamWriter sw = new StreamWriter(writeFile);
                    // open read file
                    StreamReader sr = new StreamReader(readFile);
                    // remove first line - column headers
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        // create instance of Movie class
                        Movie movie = new Movie();
                        string line = sr.ReadLine();
                        // look for quote(") in string
                        // this indicates a comma(,) or quote(") in movie title
                        int idx = line.IndexOf('"');
                        string genres = "";
                        if (idx == -1)
                        {
                            // no quote = no comma or quote in movie title
                            // movie details are separated with comma(,)
                            string[] movieDetails = line.Split(',');
                            movie.MediaId = UInt64.Parse(movieDetails[0]);
                            movie.Title = movieDetails[1];
                            genres = movieDetails[2];
                            movie.Director = movieDetails.Length > 3 ? movieDetails[3] : "unassigned";
                            movie.RunningTime = movieDetails.Length > 4 ? TimeSpan.Parse(movieDetails[4]) : new TimeSpan(0);
                        }
                        else
                        {
                            // quote = comma or quotes in movie title
                            // extract the movieId
                            movie.MediaId = UInt64.Parse(line.Substring(0, idx - 1));
                            // remove movieId and first comma from string
                            line = line.Substring(idx);
                            // find the last quote
                            idx = line.LastIndexOf('"');
                            // extract title
                            movie.Title = line.Substring(0, idx + 1);
                            // remove title and next comma from the string
                            line = line.Substring(idx + 2);
                            // split the remaining string based on commas
                            string[] details = line.Split(',');
                            // the first item in the array should be genres 
                            genres = details[0];
                            // if there is another item in the array it should be director
                            movie.Director = details.Length > 1 ? details[1] : "unassigned";
                            // if there is another item in the array it should be run time
                            movie.RunningTime = details.Length > 2 ? TimeSpan.Parse(details[2]) : new TimeSpan(0);
                        }
                        sw.WriteLine($"{movie.MediaId},{movie.Title},{genres},{movie.Director},{movie.RunningTime}");
                    }
                    sw.Close();
                    sr.Close();
                    _logger.Info("File scrub ended");
                }
                return writeFile;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return "";
        }
    }
}
