using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TraktRater
{
    public class Settings
    {
        public static string TraktUsername { get; set; }

        public static string TraktPassword { get; set; }

        public static string MovieDbUsername { get; set; }

        public static string TVDbAccountIdentifier { get; set; }

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
    }
}
