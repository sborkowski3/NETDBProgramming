using System;
using System.Collections.Generic;

namespace MediaLibrary.Models.Media
{
    public abstract class BaseMedia
    {
        // public properties
        public UInt64 MediaId { get; set; }
        public string Title { get; set; }
        public List<string> Genres { get; set; }

        // default constructor
        public BaseMedia()
        {
            Genres = new List<string>();
        }

        // the virtual method can either be used as exists here in the base class or 
        // overridden by the individual concrete classes
        public virtual string Display()
        {
            return $"Id: {MediaId}\nTitle: {Title}\nGenres: {string.Join(", ", Genres)}\n";
        }
    }
}
