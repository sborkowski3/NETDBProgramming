using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaLibrary.Models.Media;

namespace MediaLibrary.Models.Files
{
    public class AlbumFile : BaseFile<Album>
    {
        public AlbumFile(string path)
        {
            MediaList = new List<Album>();
            FilePath = path;
            // to populate the list with data, read from the data file
            try
            {
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(FilePath);
                    while (!sr.EndOfStream)
                    {
                        // create instance of Album class
                        Album album = new Album();
                        string line = sr.ReadLine();
                        // first look for quote(") in string
                        // this indicates a comma(,) in album title
                        int idx = line.IndexOf('"');
                        if (idx == -1)
                        {
                            // no quote = no comma in album title
                            // album details are separated with comma(,)
                            string[] albumDetails = line.Split(',');
                            album.MediaId = UInt64.Parse(albumDetails[0]);
                            album.Title = albumDetails[1];
                            album.Genres = albumDetails[2].Split('|').ToList();
                            album.Artist = albumDetails[3];
                            album.RecordLabel = albumDetails[4];
                        }
                        else
                        {
                            // quote = comma or quotes in album title
                            // extract the albumId
                            album.MediaId = UInt64.Parse(line.Substring(0, idx - 1));
                            // remove albumId and first comma from string
                            line = line.Substring(idx);
                            // find the last quote
                            idx = line.LastIndexOf('"');
                            // extract title
                            album.Title = line.Substring(0, idx + 1);
                            // remove title and next comma from the string
                            line = line.Substring(idx + 2);
                            // split the remaining string based on commas
                            string[] details = line.Split(',');
                            // the first item in the array should be genres 
                            album.Genres = details[0].Split('|').ToList();
                            // the next item in the array should be artist
                            album.Artist = details[1];
                            // the next item in the array should be record label
                            album.RecordLabel = details[2];
                        }
                        MediaList.Add(album);
                    }
                    // close file when done
                    sr.Close();
                    Logger.Info("Albums in file {Count}", MediaList.Count);
                }
                else
                {
                    Logger.Info("The file does not exist {Path}", path);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }

        public override bool IsUniqueTitle(string title)
        {
            if (MediaList.ConvertAll(a => a.Title.ToLower()).Contains(title.ToLower()))
            {
                Logger.Info("Duplicate album title {Title}", title);
                return false;
            }
            return true;
        }

        public override void Add(Album album)
        {
            try
            {
                // first generate album id
                album.MediaId = MediaList.Count == 0 ? 1 : MediaList.Max(a => a.MediaId) + 1;
                // if title contains a comma, wrap it in quotes
                string title = album.Title.IndexOf(',') != -1 || album.Title.IndexOf('"') != -1 ? $"\"{album.Title}\"" : album.Title;
                StreamWriter sw = new StreamWriter(FilePath, true);
                // write album data to file
                sw.WriteLine($"{album.MediaId},{title},{string.Join("|", album.Genres)},{album.Artist},{album.RecordLabel}");
                sw.Close();
                // add album details to List
                MediaList.Add(album);
                // log transaction
                Logger.Info("Media id {Id} added", album.MediaId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
