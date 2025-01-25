using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EightTube.UI
{
    public sealed partial class SearchDialog : ContentDialog
    {
        private string query;
        private int type;
        private ContentDialogResult result;

        public string Query { get { return this.query; } }
        public int Type { get { return this.type; } }
        public ContentDialogResult Result { get { return this.result; } }

        public SearchDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            query = QueryBox.Text;
            type = TypeBox.SelectedIndex;
            result = ContentDialogResult.Primary; 
            this.Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            query = null;
            type = -1;
            result = ContentDialogResult.Secondary;
            this.Hide();
        }
    }
}
