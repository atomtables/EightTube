using EightTube.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EightTube.UI
{
    public class SearchableTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ChannelTemplate { get; set; }
        public DataTemplate VideoTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is Channel)
                return ChannelTemplate;
            else if (item is Video)
                return VideoTemplate;

            return base.SelectTemplateCore(item, container);
        }
    }
}
