using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraktRater.Sites.API.IMDb
{
    public class IMDbEpisode
    {
        public string ShowName { get; set; }

        public string ShowImdbId { get; set; }

        public string EpisodeName { get; set; }

        public int SeasonNumber { get; set; }

        public int EpisodeNumber { get; set; }

        public int TvdbId { get; set; }

        public string ImdbId { get; set; }

        public string Created { get; set; }

        public int Rating { get; set; }
    }
}
