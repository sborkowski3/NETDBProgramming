using System.Collections.Generic;
using System.Linq;
using MediaLibrary.Models.Media;

namespace MediaLibrary.Models.Files
{
    public abstract class BaseFile<T> where T : BaseMedia
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public string FilePath { get; set; }
        public List<T> MediaList { get; set; }

        // some are abstract, meaning the concrete class needs to implement
        public abstract bool IsUniqueTitle(string title);

        public abstract void Add(T media);

        // some are implemented here in the base class and can be used by all inheriting classes
        public List<T> Search(string searchString)
        {
            return MediaList.Where(m => m.Title.Contains(searchString)).ToList();
        }

    }
}