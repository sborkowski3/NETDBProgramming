using System;
using System.Collections;
using System.Collections.Generic;
using MediaLibrary.Models.Files;
using MediaLibrary.Models.Media;
using MediaLibrary.Utils;
using NLog;
using System.Linq;

namespace MediaLibrary
{
    class MainClass
    {
        // create a class level instance of logger (can be used in methods other than Main)
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            Logger.Info("Program started");

            string scrubbedFile = FileScrubber.ScrubMovies("../../Data/movies.csv");
            string albumFileName = "../../Data/albums.csv";
            string bookFileName = "../../Data/books.csv";
            BaseFile<Movie> movieFile = new MovieFile(scrubbedFile);
            BaseFile<Album> albumFile = new AlbumFile(albumFileName);
            BaseFile<Book> bookFile = new BookFile(bookFileName);

            string choice = "";
            do
            {
                // display choices to user
                Console.WriteLine("1) Add Movie");
                Console.WriteLine("2) Display All Movies");
                Console.WriteLine("3) Add Album");
                Console.WriteLine("4) Display All Albums");
                Console.WriteLine("5) Add Book");
                Console.WriteLine("6) Display All Books");
                Console.WriteLine("7) Search");
                Console.WriteLine("Enter to quit");
                // input selection
                choice = Console.ReadLine();
                Logger.Info("User choice: {Choice}", choice);
                if (choice == "1")
                {
                    // Add movie
                    Movie movie = new Movie();
                    // ask user to input movie title
                    Console.WriteLine("Enter movie title");
                    // input title
                    movie.Title = Console.ReadLine();
                    // verify title is unique
                    if (movieFile.IsUniqueTitle(movie.Title))
                    {
                        // input genres
                        string input;
                        do
                        {
                            // ask user to enter genre
                            Console.WriteLine("Enter genre (or done to quit)");
                            // input genre
                            input = Console.ReadLine();
                            // if user enters "done"
                            // or does not enter a genre do not add it to list
                            if (input != "done" && input.Length > 0)
                            {
                                movie.Genres.Add(input);
                            }
                        } while (input != "done");
                        // specify if no genres are entered
                        if (movie.Genres.Count == 0)
                        {
                            movie.Genres.Add("(no genres listed)");
                        }
                        // ask user to enter director
                        Console.WriteLine("Enter movie director");
                        input = Console.ReadLine();
                        movie.Director = input.Length == 0 ? "unassigned" : input;
                        // ask user to enter running time
                        Console.WriteLine("Enter running time (h:m:s)");
                        input = Console.ReadLine();
                        movie.RunningTime = input.Length == 0 ? new TimeSpan(0) : TimeSpan.Parse(input);
                        // add movie
                        movieFile.Add(movie);
                    }
                    else
                    {
                        Console.WriteLine("Movie title already exists\n");
                    }

                }
                else if (choice == "2")
                {
                    // Display All Movies
                    foreach (Movie m in movieFile.MediaList)
                    {
                        Console.WriteLine(m.Display());
                    }
                }
                else if (choice == "3")
                {
                    // Add Album
                    Album album = new Album();
                    // ask user to input album title
                    Console.WriteLine("Enter album title");
                    // input title
                    album.Title = Console.ReadLine();
                    // verify title is unique
                    if (albumFile.IsUniqueTitle(album.Title))
                    {
                        // input genres
                        string input;
                        do
                        {
                            // ask user to enter genre
                            Console.WriteLine("Enter genre (or done to quit)");
                            // input genre
                            input = Console.ReadLine();
                            // if user enters "done"
                            // or does not enter a genre do not add it to list
                            if (input != "done" && input.Length > 0)
                            {
                                album.Genres.Add(input);
                            }
                        } while (input != "done");
                        // specify if no genres are entered
                        if (album.Genres.Count == 0)
                        {
                            album.Genres.Add("(no genres listed)");
                        }
                        // ask user to enter director
                        Console.WriteLine("Enter album artist");
                        input = Console.ReadLine();
                        album.Artist = input.Length == 0 ? "unassigned" : input;
                        // ask user to enter record label
                        Console.WriteLine("Enter record label");
                        input = Console.ReadLine();
                        album.RecordLabel = input.Length == 0 ? "unassigned" : input;
                        // add album
                        albumFile.Add(album);
                    }
                    else
                    {
                        Console.WriteLine("Album title already exists\n");
                    }
                }
                else if (choice == "4")
                {
                    // Display All Albums
                    foreach (Album a in albumFile.MediaList)
                    {
                        Console.WriteLine(a.Display());
                    }
                }
                else if (choice == "5")
                {
                    // Add Book
                    Book book = new Book();
                    // ask user to input book title
                    Console.WriteLine("Enter book title");
                    // input title
                    book.Title = Console.ReadLine();
                    // verify title is unique
                    if (bookFile.IsUniqueTitle(book.Title))
                    {
                        // input genres
                        string input;
                        do
                        {
                            // ask user to enter genre
                            Console.WriteLine("Enter genre (or done to quit)");
                            // input genre
                            input = Console.ReadLine();
                            // if user enters "done"
                            // or does not enter a genre do not add it to list
                            if (input != "done" && input.Length > 0)
                            {
                                book.Genres.Add(input);
                            }
                        } while (input != "done");
                        // specify if no genres are entered
                        if (book.Genres.Count == 0)
                        {
                            book.Genres.Add("(no genres listed)");
                        }
                        // ask user to enter author
                        Console.WriteLine("Enter book author");
                        input = Console.ReadLine();
                        book.Author = input.Length == 0 ? "unassigned" : input;
                        // ask user to enter publisher
                        Console.WriteLine("Enter publisher");
                        input = Console.ReadLine();
                        book.Publisher = input.Length == 0 ? "unassigned" : input;
                        // ask user to enter number of pages
                        Console.WriteLine("Enter number of pages");
                        input = Console.ReadLine();
                        book.PageCount = input.Length == 0 ? (UInt16)0 : UInt16.Parse(input);
                        // add book
                        bookFile.Add(book);
                    }
                    else
                    {
                        Console.WriteLine("Book title already exists\n");
                    }
                }
                else if (choice == "6")
                {
                    // Display All Books
                    foreach (Book b in bookFile.MediaList)
                    {
                        Console.WriteLine(b.Display());
                    }
                }
                else if (choice == "7")
                {
                    char selection;

                    // ask user for search details
                    Console.WriteLine("What would you like to search?");
                    Console.WriteLine($"(A)lbum, (B)ook, (M)ovie?");
                    while (!IsValidInput(Console.ReadKey(true).KeyChar, out selection))
                    {
                        Console.WriteLine($"Invalid input: {selection}");
                        Console.WriteLine();
                        Console.WriteLine("Please enter (A)lbum, (B)ook, or (M)ovie");
                        Console.WriteLine("?");
                    }

                    Console.WriteLine("Enter your search term");
                    var searchString = Console.ReadLine();

                    // create base class variable to hold results
                    IEnumerable<BaseMedia> results;

                    // call specific method on the appropriate concrete type
                    if (selection == 'A')
                        results = albumFile.Search(searchString);
                    else if (selection == 'B')
                        results = bookFile.Search(searchString);
                    else
                        results = movieFile.Search(searchString);

                    // output results
                    foreach (var item in results)
                    {
                        Console.WriteLine(item.Display());
                    }
                }
            } while (choice == "1" || choice == "2" || choice == "3" || choice == "4" || choice == "5" || choice == "6" || choice == "7");

            Logger.Info("Program ended");
        }
        private static bool IsValidInput(char input, out char selection)
        {
            char[] validValues = { 'A', 'a', 'B', 'b', 'M', 'm' };

            selection = Char.ToUpper(input);
            if (validValues.Contains(input))
            {
                return true;
            }

            return false;
        }
    }
}
