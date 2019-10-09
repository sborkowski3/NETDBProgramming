namespace MediaLibrary.Models.Media
{
    public class Album : BaseMedia
    {
        public string Artist { get; set; }
        public string RecordLabel { get; set; }

        public override string Display()
        {
            return $"Id: {MediaId}\nTitle: {Title}\nArtist: {Artist}\nLabel: {RecordLabel}\nGenres: {string.Join(", ", Genres)}\n";
        }
    }
}