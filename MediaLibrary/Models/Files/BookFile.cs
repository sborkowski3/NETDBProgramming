using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaLibrary.Models.Media;

namespace MediaLibrary.Models.Files
{
    public class BookFile : BaseFile<Book>
    {
        public BookFile(string path)
        {
            MediaList = new List<Book>();
            FilePath = path;
            // to populate the list with data, read from the data file
            try
            {
                if (File.Exists(path))
                {
                    StreamReader sr = new StreamReader(FilePath);
                    while (!sr.EndOfStream)
                    {
                        // create instance of Book class
                        Book book = new Book();
                        string line = sr.ReadLine();
                        // first look for quote(") in string
                        // this indicates a comma(,) in book title
                        int idx = line.IndexOf('"');
                        if (idx == -1)
                        {
                            // no quote = no comma in album title
                            // book details are separated with comma(,)
                            string[] bookDetails = line.Split(',');
                            book.MediaId = UInt64.Parse(bookDetails[0]);
                            book.Title = bookDetails[1];
                            book.Genres = bookDetails[2].Split('|').ToList();
                            book.Author = bookDetails[3];
                            book.Publisher = bookDetails[4];
                            book.PageCount = UInt16.Parse(bookDetails[5]);
                        }
                        else
                        {
                            // quote = comma or quotes in book title
                            // extract the bookId
                            book.MediaId = UInt64.Parse(line.Substring(0, idx - 1));
                            // remove bookId and first comma from string
                            line = line.Substring(idx);
                            // find the last quote
                            idx = line.LastIndexOf('"');
                            // extract title
                            book.Title = line.Substring(0, idx + 1);
                            // remove title and next comma from the string
                            line = line.Substring(idx + 2);
                            // split the remaining string based on commas
                            string[] details = line.Split(',');
                            // the first item in the array should be genres 
                            book.Genres = details[0].Split('|').ToList();
                            // the next item in the array should be author
                            book.Author = details[1];
                            // the next item in the array should be publisher
                            book.Publisher = details[2];
                            // the next item in the array should be page count
                            book.PageCount = UInt16.Parse(details[3]);
                        }
                        MediaList.Add(book);
                    }
                    // close file when done
                    sr.Close();
                    Logger.Info("Books in file {Count}", MediaList.Count);
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
            if (MediaList.ConvertAll(b => b.Title.ToLower()).Contains(title.ToLower()))
            {
                Logger.Info("Duplicate book title {Title}", title);
                return false;
            }
            return true;
        }

        public override void Add(Book book)
        {
            try
            {
                // first generate book id
                book.MediaId = MediaList.Count == 0 ? 1 : MediaList.Max(b => b.MediaId) + 1;
                // if title contains a comma, wrap it in quotes
                string title = book.Title.IndexOf(',') != -1 || book.Title.IndexOf('"') != -1 ? $"\"{book.Title}\"" : book.Title;
                StreamWriter sw = new StreamWriter(FilePath, true);
                // write book data to file
                sw.WriteLine($"{book.MediaId},{title},{string.Join("|", book.Genres)},{book.Author},{book.Publisher},{book.PageCount}");
                sw.Close();
                // add book details to List
                MediaList.Add(book);
                // log transaction
                Logger.Info("Media id {Id} added", book.MediaId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
    }
}
