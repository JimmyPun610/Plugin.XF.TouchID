using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.XF.TouchID.Abstractions
{
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension, INotifyPropertyChanged
    {
        internal string _source { get; set; }
        public string Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = $"Plugin.XF.TouchID.Abstractions.{value}";
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,
                          new PropertyChangedEventArgs("Source"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }
    }
}
