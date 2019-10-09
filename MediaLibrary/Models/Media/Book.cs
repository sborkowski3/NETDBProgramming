using System;

namespace MediaLibrary.Models.Media
{
    public class Book : BaseMedia
    {
        public string Author { get; set; }
        public UInt16 PageCount { get; set; }
        public string Publisher { get; set; }

        public override string Display()
        {
            return $"Id: {MediaId}\nTitle: {Title}\nAuthor: {Author}\nPages: {PageCount}\nPublisher: {Publisher}\nGenres: {string.Join(", ", Genres)}\n";
        }
    }
}