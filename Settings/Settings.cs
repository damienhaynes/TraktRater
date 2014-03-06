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
        const string cIMDbFilename = "IMDbFilename";
        const string cIMDbUsername = "IMDbUsername";
        const string cIMDBSyncWatchlist = "IMDBSyncWatchlist";
        const string cMarkAsWatched = "MarkAsWatched";
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

        public static string IMDbFilename { get; set; }

        public static bool IMDbSyncWatchlist { get; set; }

        public static string IMDbUsername { get; set; }

        public static bool MarkAsWatched { get; set; }

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
            IMDbFilename = xmlReader.GetSettingValueAsString(cIMDbFilename, string.Empty);
            IMDbUsername = xmlReader.GetSettingValueAsString(cIMDbUsername, string.Empty);
            IMDbSyncWatchlist = xmlReader.GetSettingValueAsBool(cIMDBSyncWatchlist, false);
            MarkAsWatched = xmlReader.GetSettingValueAsBool(cMarkAsWatched, true);

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
            xmlWriter.WriteSetting(cIMDbFilename, IMDbFilename);
            xmlWriter.WriteSetting(cIMDbUsername, IMDbUsername);
            xmlWriter.WriteSetting(cIMDBSyncWatchlist, IMDbSyncWatchlist.ToString());
            xmlWriter.WriteSetting(cMarkAsWatched, MarkAsWatched.ToString());

            // save file
            xmlWriter.Save(SettingsFile);
        }
        #endregion

    }
}
