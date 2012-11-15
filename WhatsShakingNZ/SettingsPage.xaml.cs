using System.Windows;
using System.Windows.Media;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;
using WhatsShakingNZ.Localization;

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
                    Title = AppResources.LiveTileTaskLimitReachedToastTitle,
                    Message = AppResources.LiveTileTaskLimitReachedToastMessage
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