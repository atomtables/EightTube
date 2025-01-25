using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace EightTube.UI
{
    public class SystemProgressIndicator
    {

        public static async Task show()
        {
            var statusBar = StatusBar.GetForCurrentView();

            statusBar.ProgressIndicator.Text = "Loading something...";
            statusBar.ProgressIndicator.ProgressValue = null;
            await statusBar.ProgressIndicator.ShowAsync();
        }

        public static async Task show(string text)
        {
            var statusBar = StatusBar.GetForCurrentView();

            statusBar.ProgressIndicator.Text = text;
            statusBar.ProgressIndicator.ProgressValue = null;
            await statusBar.ProgressIndicator.ShowAsync();
        }

        public static async Task show(string text, int progress)
        {
            var statusBar = StatusBar.GetForCurrentView();

            statusBar.ProgressIndicator.Text = text;
            statusBar.ProgressIndicator.ProgressValue = progress;
            await statusBar.ProgressIndicator.ShowAsync();
        }

        public static async Task hide()
        {
            await StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
        }
    }
}
