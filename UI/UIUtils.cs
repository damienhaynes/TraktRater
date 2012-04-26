using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TraktRater;

namespace TraktRater.UI
{
    delegate void UpdateProgressDelegate(string message, bool error);

    public static class UIUtils
    {
        public static void UpdateStatus(string message)
        {
            UpdateStatus(message, false);
        }

        public static void UpdateStatus(string message, bool error)
        {   
            if (Program.MainWindow.InvokeRequired)
            {
                UpdateProgressDelegate updateProgress = new UpdateProgressDelegate(UpdateStatus);
                object[] args = { message, error };
                Program.MainWindow.Invoke(updateProgress, args);
                return;
            }

            Program.MainWindow.lblStatusMessage.Text = message;
            Program.MainWindow.lblStatusMessage.ForeColor = error ? Color.Red : Color.Black;
        }
    }
}
