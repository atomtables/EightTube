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
    public sealed partial class VideoQualityDialog : ContentDialog
    {
        public List<String> source { get; set; }

        private ContentDialogResult result;
        private int index;

        public ContentDialogResult Result { get { return result; } }
        public int Index { get { return index; } }

        public VideoQualityDialog()
        {
            this.InitializeComponent();
        }

        public void SetItems(List<String> source)
        {
            this.source = source;
            QualityBox.ItemsSource = source;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            result = ContentDialogResult.Primary;
            index = QualityBox.SelectedIndex;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            result = ContentDialogResult.Secondary;
            index = -1;
        }
    }
}
