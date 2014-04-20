using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TraktRater.Settings.XML;

namespace TraktRater.Settings
{
    public class AppSettings
    {
        #region Constants
        const string cTraktUsername = "TraktUsername";
        const string cTraktPassword = "TraktPassword";
        const string cTVDbAccountId = "TVDbAccountId";
        const string cTMDbSessionId = "TMDbSessionId";
        const string cIMDbRatingsFilename = "IMDbFilename";
        const string cIMDbWatchlistFilename = "IMDbWatchlistFilename";
        const string cIMDbUsername = "IMDbUsername";
        const string cIMDBSyncWatchlist = "IMDBSyncWatchlist";
        const string cTMDBSyncWatchlist = "TMDBSyncWatchlist";
        const string cListalSyncWatchlist = "ListalSyncWatchlist";
        const string cListalMovieFilename = "ListalMovieFilename";
        const string cListalShowFilename = "ListalShowFilename";
        const string cCritickerMovieFilename = "CritickerMovieFilename";
        const string cMarkAsWatched = "MarkAsWatched";
        const string cIgnoreWatchedForWatchlist = "IgnoreWatchedForWatchlist";
        const string cEnableTMDb = "EnableTMDb";
        const string cEnableTVDb = "EnableTVDb";
        const string cEnableIMDb = "EnableIMDb";
        const string cEnableListal = "EnableListal";
        const string cEnableCriticker = "EnableCriticker";
        #endregion

        #region Settings
        public static string TraktUsername
        {
            get
            {
                return _traktUsername;
            }
            set
            {
                _traktUsername = value;
                TraktAPI.TraktAPI.Username = _traktUsername;
            }
        }
        static string _traktUsername = null;

        public static string TraktPassword
        {
            get
            {
                return _traktPassword;
            }
            set
            {
                _traktPassword = value;
                TraktAPI.TraktAPI.Password = _traktPassword;
            }
        }
        static string _traktPassword = null;

        public static string TVDbAccountIdentifier { get; set; }

        public static string TMDbSessionId { get; set; }

        public static string TMDbRequestToken { get; set; }

        public static string IMDbRatingsFilename { get; set; }
        
        public static string IMDbWatchlistFilename { get; set; }

        public static bool IMDbSyncWatchlist { get; set; }

        public static bool TMDbSyncWatchlist { get; set; }

        public static bool ListalSyncWatchlist { get; set; }

        public static string IMDbUsername { get; set; }

        public static string ListalMovieFilename { get; set; }

        public static string ListalShowFilename { get; set; }

        public static string CritickerMovieFilename { get; set; }

        public static bool MarkAsWatched { get; set; }

        public static bool IgnoreWatchedForWatchlist { get; set; }

        public static bool EnableTVDb { get; set; }
        public static bool EnableTMDb { get; set; }
        public static bool EnableIMDb { get; set; }
        public static bool EnableListal { get; set; }
        public static bool EnableCriticker { get; set; }

        public static string SettingsFile
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TraktRater\Settings.xml";
            }
        }

        public static string UserAgent
        {
            get
            {
                return string.Format("TraktRater/{0}", Version);
            }
        }

        public static string Version
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
            XmlReader xmlReader = new XmlReader();
            xmlReader.Load(SettingsFile);

            TraktUsername = xmlReader.GetSettingValueAsString(cTraktUsername, string.Empty);
            TraktPassword = xmlReader.GetSettingValueAsString(cTraktPassword, string.Empty);
            TVDbAccountIdentifier = xmlReader.GetSettingValueAsString(cTVDbAccountId, string.Empty);
            TMDbSessionId = xmlReader.GetSettingValueAsString(cTMDbSessionId, string.Empty);
            TMDbSyncWatchlist = xmlReader.GetSettingValueAsBool(cTMDBSyncWatchlist, true);
            IMDbRatingsFilename = xmlReader.GetSettingValueAsString(cIMDbRatingsFilename, string.Empty);
            IMDbWatchlistFilename = xmlReader.GetSettingValueAsString(cIMDbWatchlistFilename, string.Empty);
            IMDbUsername = xmlReader.GetSettingValueAsString(cIMDbUsername, string.Empty);
            IMDbSyncWatchlist = xmlReader.GetSettingValueAsBool(cIMDBSyncWatchlist, false);
            ListalSyncWatchlist = xmlReader.GetSettingValueAsBool(cListalSyncWatchlist, false);
            ListalMovieFilename = xmlReader.GetSettingValueAsString(cListalMovieFilename, string.Empty);
            ListalShowFilename = xmlReader.GetSettingValueAsString(cListalShowFilename, string.Empty);
            CritickerMovieFilename = xmlReader.GetSettingValueAsString(cCritickerMovieFilename, string.Empty);
            MarkAsWatched = xmlReader.GetSettingValueAsBool(cMarkAsWatched, true);
            IgnoreWatchedForWatchlist = xmlReader.GetSettingValueAsBool(cIgnoreWatchedForWatchlist, true);
            EnableIMDb = xmlReader.GetSettingValueAsBool(cEnableIMDb, false);
            EnableTMDb = xmlReader.GetSettingValueAsBool(cEnableTMDb, false);
            EnableTVDb = xmlReader.GetSettingValueAsBool(cEnableTVDb, false);
            EnableListal = xmlReader.GetSettingValueAsBool(cEnableListal, false);
            EnableCriticker = xmlReader.GetSettingValueAsBool(cEnableCriticker, false);

            // save settings, might be some new settings added
            Save();
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

            xmlWriter.WriteSetting(cTraktUsername, TraktUsername);
            xmlWriter.WriteSetting(cTraktPassword, TraktPassword);
            xmlWriter.WriteSetting(cTVDbAccountId, TVDbAccountIdentifier);
            xmlWriter.WriteSetting(cTMDbSessionId, TMDbSessionId);
            xmlWriter.WriteSetting(cTMDBSyncWatchlist, TMDbSyncWatchlist.ToString());
            xmlWriter.WriteSetting(cIMDbRatingsFilename, IMDbRatingsFilename);
            xmlWriter.WriteSetting(cIMDbWatchlistFilename, IMDbWatchlistFilename);
            xmlWriter.WriteSetting(cIMDbUsername, IMDbUsername);
            xmlWriter.WriteSetting(cIMDBSyncWatchlist, IMDbSyncWatchlist.ToString());
            xmlWriter.WriteSetting(cListalSyncWatchlist, ListalSyncWatchlist.ToString());
            xmlWriter.WriteSetting(cListalMovieFilename, ListalMovieFilename);
            xmlWriter.WriteSetting(cListalShowFilename, ListalShowFilename);
            xmlWriter.WriteSetting(cCritickerMovieFilename, CritickerMovieFilename);
            xmlWriter.WriteSetting(cMarkAsWatched, MarkAsWatched.ToString());
            xmlWriter.WriteSetting(cIgnoreWatchedForWatchlist, IgnoreWatchedForWatchlist.ToString());
            xmlWriter.WriteSetting(cEnableIMDb, EnableIMDb.ToString());
            xmlWriter.WriteSetting(cEnableTMDb, EnableTMDb.ToString());
            xmlWriter.WriteSetting(cEnableTVDb, EnableTVDb.ToString());
            xmlWriter.WriteSetting(cEnableListal, EnableListal.ToString());
            xmlWriter.WriteSetting(cEnableCriticker, EnableCriticker.ToString());

            // save file
            xmlWriter.Save(SettingsFile);
        }
        #endregion

    }
}
