namespace TraktRater
{
    using System;
    using System.Windows.Forms;

    static class Program
    {
        public static TraktRaterApp MainWindow;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainWindow = new TraktRaterApp();
            Application.Run(MainWindow);
        }
    }
}
