using System.Windows;
using System.Windows.Media;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;
using WhatsShakingNZ.Localization;
using TileControls;

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

        private void LiveTileToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            if(!ScheduledTaskHelper.Add())
            {
                ToastPrompt toast = new ToastPrompt()
                {
                    TextOrientation = System.Windows.Controls.Orientation.Vertical,
                    Title = AppResources.LiveTileTaskLimitReachedToastTitle,
                    Message = AppResources.LiveTileTaskLimitReachedToastMessage,
                    TextWrapping = TextWrapping.Wrap
                };
                toast.Show();
                LiveTileToggle.IsChecked = false;
            }
        }

        private void LiveTileToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            ScheduledTaskHelper.Remove();
            var ftc = new FlipTileController();
            ftc.ClearFlipTile();
        }

        private void GeonetEndpointToggle_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as ToggleSwitch).IsChecked ?? false)
            {
                ToastPrompt toast = new ToastPrompt()
                {
                    TextOrientation = System.Windows.Controls.Orientation.Vertical,
                    Title = AppResources.GeonetAllQuakesEndpointSettingEnabledWarningToastTitle,
                    Message = AppResources.GeonetAllQuakesEndpointSettingEnabledWarningToastMessage,
                    TextWrapping = TextWrapping.Wrap,
                };
                toast.Show();
            }
        }
    }
}