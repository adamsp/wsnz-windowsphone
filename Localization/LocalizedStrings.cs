using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WhatsShakingNZ.Localization
{
    /// <summary>
    /// As per this MSDN guide: http://msdn.microsoft.com/en-us/library/ff637520(v=vs.92).aspx
    /// </summary>
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {
        }

        private static AppResources _localizedResources = new AppResources();
        public AppResources LocalizedResources
        {
            get
            {
                return _localizedResources;
            }
        }
    }
}
