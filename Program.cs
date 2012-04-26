using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TraktRater
{
    static class Program
    {
        public static TraktRater MainWindow;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainWindow = new TraktRater();
            Application.Run(MainWindow);
        }
    }
}
