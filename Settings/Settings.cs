namespace TraktRater.Settings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using global::TraktRater.Extensions;
    using global::TraktRater.Settings.XML;
    using global::TraktRater.TraktAPI;

    internal class AppSettings
    {
        public enum LoggingSeverity
        {
            Error,
            Warning,
            Info,
            Debug,
            Trace
        }

        #region Constants
        const string cTraktOAuthToken = "TraktOAuthToken";
        const string cTVDbAccountId = "TVDbAccountId";
        const string cTMDbSessionId = "TMDbSessionId";
        const string cIMDbRatingsFilename = "IMDbFilename";
        const string cIMDbWatchlistFilename = "IMDbWatchlistFilename";
        const string cIMDbUsername = "IMDbUsername";
        const string cIMDbCustomLists = "IMDbCustomLists";
        const string cIMDBSyncWatchlist = "IMDBSyncWatchlist";
        const string cTMDBSyncWatchlist = "TMDBSyncWatchlist";
        const string cListalSyncWatchlist = "ListalSyncWatchlist";
        const string cListalMovieFilename = "ListalMovieFilename";
        const string cListalShowFilename = "ListalShowFilename";
        const string cCritickerCSVFilename = "CritickerCSVFilename";
        const string cToDoMovieFilename = "ToDoMovieFilename";
        const string cLetterboxdRatingsFilename = "LetterboxdRatingsFilename";
        const string cLetterboxdWatchedFilename = "LetterboxdWatchedFilename";
        const string cLetterboxdDiaryFilename = "LetterboxdDiaryFilename";
        const string cLetterboxdWatchlistFilename = "LetterboxdWatchlistFilename";
        const string cLetterboxdCustomLists = "LetterboxdCustomLists";
        const string cFlixsterUserId = "FlixsterUserId";
        const string cFlixsterSyncWantToSee = "FlixsterSyncWantToSee";
        const string cMarkAsWatched = "MarkAsWatched";
        const string cIgnoreWatchedForWatchlist = "IgnoreWatchedForWatchlist";
        const string cCheckMoviesFilename = "CheckMoviesFilename";
        const string cCheckMoviesAddWatchedMoviesToWatchlist = "CheckMoviesAddWatchedMoviesToWatchlist";
        const string cCheckMoviesUpdateWatchedHistory = "CheckMoviesUpdateWatchedHistory";
        const string cCheckMoviesDelimiter = "CheckMoviesDelimiter";
        const string cCheckMoviesAddToCollection = "CheckMoviesAddToCollection";
        const string cEnableTMDb = "EnableTMDb";
        const string cEnableTVDb = "EnableTVDb";
        const string cEnableIMDb = "EnableIMDb";
        const string cEnableListal = "EnableListal";
        const string cEnableCriticker = "EnableCriticker";
        const string cEnableLetterboxd = "EnableLetterboxd";
        const string cEnableFlixster = "EnableFlixster";
        const string cEnableCheckMovies = "EnableCheckMovies";
        const string cEnableToDoMovies = "EnableToDoMovies";
        const string cEnableMovieLens = "EnableMovieLens";
        const string cLogLevel = "LogLevel";
        const string cBatchSize = "BatchSize";
        const string cWatchedOnReleaseDay = "WatchedOnReleaseDay";
        const string cCsvExportItems = "CsvExportItems";
        const string cCsvExportPath = "CsvExportPath";
        const string cMovieLensRatingsFilename = "MovieLensRatingsFilename";
        const string cMovieLensWishListFilename = "MovieLensWishListFilename";
        const string cMovieLensActivityLogFilename = "MovieLensActivityLogFilename";
        const string cMovieLensTagsFilename = "MovieLensTagFilename";
        #endregion

        #region Settings
        public static string TraktOAuthToken { get; set; }
        
        public static string TVDbAccountIdentifier { get; set; }

        public static string TMDbSessionId { get; set; }

        public static string TMDbRequestToken { get; set; }

        public static bool CheckMoviesAddWatchedMoviesToWatchlist;

        public static bool CheckMoviesUpdateWatchedHistory;

        public static string CheckMoviesFilename { get; set; }

        public static int CheckMoviesDelimiter { get; set; }

        public static bool CheckMoviesAddToCollection { get; set; }

        public static string IMDbRatingsFilename { get; set; }
        
        public static string IMDbWatchlistFilename { get; set; }

        public static List<string> IMDbCustomLists { get; set; }

        public static bool IMDbSyncWatchlist { get; set; }

        public static bool TMDbSyncWatchlist { get; set; }

        public static bool ListalSyncWatchlist { get; set; }

        public static string IMDbUsername { get; set; }

        public static string ListalMovieFilename { get; set; }

        public static string ListalShowFilename { get; set; }

        public static string CritickerCSVFilename { get; set; }

        public static string ToDoMovieFilename { get; set; }

        public static string LetterboxdRatingsFilename { get; set; }
        
        public static string LetterboxdWatchedFilename { get; set; }

        public static string LetterboxdWatchlistFilename { get; set; }

        public static string LetterboxdDiaryFilename { get; set; }

        public static List<string> LetterboxdCustomLists { get; set; }

        public static string FlixsterUserId { get; set; }

        public static bool FlixsterSyncWantToSee { get; set; }

        public static string MovieLensRatingsFilename { get; set; }

        public static string MovieLensWishListFilename { get; set; }

        public static string MovieLensActivityFilename { get; set; }
        
        public static string MovieLensTagsFilename { get; set; }

        public static bool MarkAsWatched { get; set; }

        public static bool IgnoreWatchedForWatchlist { get; set; }

        public static int BatchSize { get; set; }

        public static bool WatchedOnReleaseDay { get; set; }

        public static bool EnableTVDb { get; set; }
        public static bool EnableTMDb { get; set; }
        public static bool EnableIMDb { get; set; }
        public static bool EnableCheckMovies { get; set; }
        public static bool EnableListal { get; set; }
        public static bool EnableCriticker { get; set; }
        public static bool EnableLetterboxd { get; set; }
        public static bool EnableFlixster { get; set; }
        public static bool EnableToDoMovies { get; set; }

        public static bool EnableMovieLens { get; set; }

        public static ExportItems CsvExportItems { get; set; }
        public static string CsvExportPath { get; set; }

        public static LoggingSeverity LogSeverityLevel { get; set; }

        private static string SettingsFile
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"TraktRater", @"Settings.xml");
            }
        }

        public static string UserAgent
        {
            get
            {
                return string.Format("TraktRater/{0}", Version);
            }
        }

        private static string Version
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().Version.ToString();
            }
        }

        #endregion

        #region Persistence
        /// <summary>
        /// Loads the Settings
        /// </summary>
        public static void Load()
        {
            try 
            {
                XmlReader xmlReader = new XmlReader();
                xmlReader.Load(SettingsFile);

                TraktOAuthToken = xmlReader.GetSettingValueAsString(cTraktOAuthToken, string.Empty);
                TVDbAccountIdentifier = xmlReader.GetSettingValueAsString(cTVDbAccountId, string.Empty);
                TMDbSessionId = xmlReader.GetSettingValueAsString(cTMDbSessionId, string.Empty);
                TMDbSyncWatchlist = xmlReader.GetSettingValueAsBool(cTMDBSyncWatchlist, true);
                IMDbRatingsFilename = xmlReader.GetSettingValueAsString(cIMDbRatingsFilename, string.Empty);
                IMDbWatchlistFilename = xmlReader.GetSettingValueAsString(cIMDbWatchlistFilename, string.Empty);
                IMDbUsername = xmlReader.GetSettingValueAsString(cIMDbUsername, string.Empty);
                IMDbCustomLists = xmlReader.GetSettingValueAsString(cIMDbCustomLists, string.Empty).FromJSONArray<string>().ToList();
                IMDbSyncWatchlist = xmlReader.GetSettingValueAsBool(cIMDBSyncWatchlist, false);
                ListalSyncWatchlist = xmlReader.GetSettingValueAsBool(cListalSyncWatchlist, false);
                ListalMovieFilename = xmlReader.GetSettingValueAsString(cListalMovieFilename, string.Empty);
                ListalShowFilename = xmlReader.GetSettingValueAsString(cListalShowFilename, string.Empty);
                CritickerCSVFilename = xmlReader.GetSettingValueAsString(cCritickerCSVFilename, string.Empty);
                ToDoMovieFilename = xmlReader.GetSettingValueAsString(cToDoMovieFilename, string.Empty);
                LetterboxdRatingsFilename = xmlReader.GetSettingValueAsString(cLetterboxdRatingsFilename, string.Empty);
                LetterboxdWatchedFilename = xmlReader.GetSettingValueAsString(cLetterboxdWatchedFilename, string.Empty);
                LetterboxdWatchlistFilename = xmlReader.GetSettingValueAsString(cLetterboxdWatchlistFilename, string.Empty);
                LetterboxdDiaryFilename = xmlReader.GetSettingValueAsString(cLetterboxdDiaryFilename, string.Empty);
                LetterboxdCustomLists = xmlReader.GetSettingValueAsString( cLetterboxdCustomLists, string.Empty ).FromJSONArray<string>().ToList();
                MovieLensRatingsFilename = xmlReader.GetSettingValueAsString(cMovieLensRatingsFilename, string.Empty);
                MovieLensWishListFilename = xmlReader.GetSettingValueAsString(cMovieLensWishListFilename, string.Empty);
                MovieLensActivityFilename = xmlReader.GetSettingValueAsString(cMovieLensActivityLogFilename, string.Empty);
                MovieLensTagsFilename = xmlReader.GetSettingValueAsString(cMovieLensTagsFilename, string.Empty);
                FlixsterUserId = xmlReader.GetSettingValueAsString(cFlixsterUserId, string.Empty);
                FlixsterSyncWantToSee = xmlReader.GetSettingValueAsBool(cFlixsterSyncWantToSee, true);
                CheckMoviesFilename = xmlReader.GetSettingValueAsString(cCheckMoviesFilename, string.Empty);
                CheckMoviesUpdateWatchedHistory = xmlReader.GetSettingValueAsBool(cCheckMoviesUpdateWatchedHistory, true);
                CheckMoviesAddWatchedMoviesToWatchlist = xmlReader.GetSettingValueAsBool(cCheckMoviesAddWatchedMoviesToWatchlist, false);
                CheckMoviesDelimiter = xmlReader.GetSettingValueAsInt(cCheckMoviesDelimiter, 0);
                CheckMoviesAddToCollection = xmlReader.GetSettingValueAsBool(cCheckMoviesAddToCollection, true);
                MarkAsWatched = xmlReader.GetSettingValueAsBool(cMarkAsWatched, true);
                IgnoreWatchedForWatchlist = xmlReader.GetSettingValueAsBool(cIgnoreWatchedForWatchlist, true);
                EnableIMDb = xmlReader.GetSettingValueAsBool(cEnableIMDb, false);
                EnableTMDb = xmlReader.GetSettingValueAsBool(cEnableTMDb, false);
                EnableTVDb = xmlReader.GetSettingValueAsBool(cEnableTVDb, false);
                EnableListal = xmlReader.GetSettingValueAsBool(cEnableListal, false);
                EnableCriticker = xmlReader.GetSettingValueAsBool(cEnableCriticker, false);
                EnableLetterboxd = xmlReader.GetSettingValueAsBool(cEnableLetterboxd, false);
                EnableFlixster = xmlReader.GetSettingValueAsBool(cEnableFlixster, false);
                EnableCheckMovies = xmlReader.GetSettingValueAsBool(cEnableCheckMovies, false);
                EnableToDoMovies = xmlReader.GetSettingValueAsBool(cEnableToDoMovies, false);
                EnableMovieLens = xmlReader.GetSettingValueAsBool(cEnableMovieLens, false);
                LogSeverityLevel = (LoggingSeverity)(xmlReader.GetSettingValueAsInt(cLogLevel, 3));
                BatchSize = xmlReader.GetSettingValueAsInt(cBatchSize, 50);
                WatchedOnReleaseDay = xmlReader.GetSettingValueAsBool(cWatchedOnReleaseDay, false);
                CsvExportItems = xmlReader.GetSettingValueAsString(cCsvExportItems, new ExportItems().ToJSON()).FromJSON<ExportItems>();
                CsvExportPath = xmlReader.GetSettingValueAsString(cCsvExportPath, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"TraktRater", @"Export"));

                // save settings, might be some new settings added
                Save();
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Saves the Settings
        /// </summary>
        public static void Save()
        {
            XmlWriter xmlWriter = new XmlWriter();
            if (!xmlWriter.Load(SettingsFile))
            {
                if (File.Exists(SettingsFile))
                {
                    try
                    {
                        File.Delete(SettingsFile);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }

                // create it and reload
                xmlWriter.CreateXmlSettings(SettingsFile);
                xmlWriter.Load(SettingsFile);
            }
            
            xmlWriter.WriteSetting(cTraktOAuthToken, TraktOAuthToken);
            xmlWriter.WriteSetting(cTVDbAccountId, TVDbAccountIdentifier);
            xmlWriter.WriteSetting(cTMDbSessionId, TMDbSessionId);
            xmlWriter.WriteSetting(cTMDBSyncWatchlist, TMDbSyncWatchlist.ToString());
            xmlWriter.WriteSetting(cIMDbRatingsFilename, IMDbRatingsFilename);
            xmlWriter.WriteSetting(cIMDbWatchlistFilename, IMDbWatchlistFilename);
            xmlWriter.WriteSetting(cIMDbUsername, IMDbUsername);
            xmlWriter.WriteSetting(cIMDBSyncWatchlist, IMDbSyncWatchlist.ToString());
            xmlWriter.WriteSetting(cIMDbCustomLists, IMDbCustomLists.ToJSON());
            xmlWriter.WriteSetting(cListalSyncWatchlist, ListalSyncWatchlist.ToString());
            xmlWriter.WriteSetting(cListalMovieFilename, ListalMovieFilename);
            xmlWriter.WriteSetting(cListalShowFilename, ListalShowFilename);
            xmlWriter.WriteSetting(cCritickerCSVFilename, CritickerCSVFilename);
            xmlWriter.WriteSetting(cToDoMovieFilename, ToDoMovieFilename);
            xmlWriter.WriteSetting(cLetterboxdRatingsFilename, LetterboxdRatingsFilename);
            xmlWriter.WriteSetting(cLetterboxdWatchedFilename, LetterboxdWatchedFilename);
            xmlWriter.WriteSetting(cLetterboxdWatchlistFilename, LetterboxdWatchlistFilename);
            xmlWriter.WriteSetting(cLetterboxdDiaryFilename, LetterboxdDiaryFilename);
            xmlWriter.WriteSetting(cLetterboxdCustomLists, LetterboxdCustomLists.ToJSON());
            xmlWriter.WriteSetting(cMovieLensRatingsFilename, MovieLensRatingsFilename);
            xmlWriter.WriteSetting(cMovieLensWishListFilename, MovieLensWishListFilename);
            xmlWriter.WriteSetting(cMovieLensActivityLogFilename, MovieLensActivityFilename);
            xmlWriter.WriteSetting(cMovieLensTagsFilename, MovieLensTagsFilename);
            xmlWriter.WriteSetting(cFlixsterUserId, FlixsterUserId);
            xmlWriter.WriteSetting(cFlixsterSyncWantToSee, FlixsterSyncWantToSee.ToString());
            xmlWriter.WriteSetting(cCheckMoviesFilename, CheckMoviesFilename);
            xmlWriter.WriteSetting(cCheckMoviesAddWatchedMoviesToWatchlist, CheckMoviesAddWatchedMoviesToWatchlist.ToString());
            xmlWriter.WriteSetting(cCheckMoviesUpdateWatchedHistory, CheckMoviesUpdateWatchedHistory.ToString());
            xmlWriter.WriteSetting(cCheckMoviesDelimiter, CheckMoviesDelimiter.ToString());
            xmlWriter.WriteSetting(cCheckMoviesAddToCollection, CheckMoviesAddToCollection.ToString());
            xmlWriter.WriteSetting(cMarkAsWatched, MarkAsWatched.ToString());
            xmlWriter.WriteSetting(cIgnoreWatchedForWatchlist, IgnoreWatchedForWatchlist.ToString());
            xmlWriter.WriteSetting(cEnableIMDb, EnableIMDb.ToString());
            xmlWriter.WriteSetting(cEnableTMDb, EnableTMDb.ToString());
            xmlWriter.WriteSetting(cEnableTVDb, EnableTVDb.ToString());
            xmlWriter.WriteSetting(cEnableListal, EnableListal.ToString());
            xmlWriter.WriteSetting(cEnableCriticker, EnableCriticker.ToString());
            xmlWriter.WriteSetting(cEnableLetterboxd, EnableLetterboxd.ToString());
            xmlWriter.WriteSetting(cEnableFlixster, EnableFlixster.ToString());
            xmlWriter.WriteSetting(cEnableCheckMovies, EnableCheckMovies.ToString());
            xmlWriter.WriteSetting(cEnableToDoMovies, EnableToDoMovies.ToString());
            xmlWriter.WriteSetting(cEnableMovieLens, EnableMovieLens.ToString());
            xmlWriter.WriteSetting(cLogLevel, ((int)LogSeverityLevel).ToString());
            xmlWriter.WriteSetting(cBatchSize, BatchSize.ToString());
            xmlWriter.WriteSetting(cWatchedOnReleaseDay, WatchedOnReleaseDay.ToString());
            xmlWriter.WriteSetting(cCsvExportItems, CsvExportItems.ToJSON());
            xmlWriter.WriteSetting(cCsvExportPath, CsvExportPath);

            // save file
            xmlWriter.Save(SettingsFile);
        }
        #endregion

    }
}
