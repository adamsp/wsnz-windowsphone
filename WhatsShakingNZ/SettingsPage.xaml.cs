using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Coding4Fun.Phone.Controls;

namespace WhatsShakingNZ
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            ImageBrush backgroundImage = new ImageBrush();
            backgroundImage.ImageSource = ShakingHelper.GetBackgroundImage();
            backgroundImage.Stretch = Stretch.None;
            backgroundImage.AlignmentY = AlignmentY.Top;
            LayoutRoot.Background = backgroundImage;
        }

        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            if(!ScheduledTaskHelper.Add())
            {
                ToastPrompt toast = new ToastPrompt()
                {
                    TextOrientation = System.Windows.Controls.Orientation.Vertical,
                    Title = "cannot use live tile",
                    Message = "scheduled task limit has been reached. please disable other apps scheduled tasks in phone settings."
                };
                toast.Show();
                LiveTileToggle.IsChecked = false;
            }
        }

        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            ScheduledTaskHelper.Remove();
        }
    }
}