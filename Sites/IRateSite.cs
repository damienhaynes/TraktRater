using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraktRater.TraktAPI.DataStructures;

namespace TraktRater.Sites
{
    interface IRateSite
    {
        /// <summary>
        /// Site is enabled for Ratings Import
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Site is enabled for Ratings Import
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Imports Ratings to trakt.tv
        /// </summary>        
        void ImportRatings();

        /// <summary>
        /// Cancels any import process
        /// </summary>
        void Cancel();
    }
}
