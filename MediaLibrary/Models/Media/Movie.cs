using System;

namespace MediaLibrary.Models.Media
{
    public class Movie : BaseMedia
    {
        public string Director { get; set; }
        public TimeSpan RunningTime { get; set; }

        public override string Display()
        {
            return $"Id: {MediaId}\nTitle: {Title}\nDirector: {Director}\nRun time: {RunningTime}\nGenres: {string.Join(", ", Genres)}\n";
        }
    }
}