using System;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using WhatsShakingNZ.Localization;

namespace WhatsShakingNZ
{
    public partial class AboutPage : PhoneApplicationPage
    {
        // Declare the MarketplaceDetailTask object with page scope
        // so we can access it from event handlers.
        private MarketplaceDetailTask _marketPlaceDetailTask = new MarketplaceDetailTask();
        private MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
        private MarketplaceSearchTask searchTask = new MarketplaceSearchTask();
        public AboutPage()
        {
            InitializeComponent();
            ImageBrush backgroundImage = new ImageBrush();
            backgroundImage.ImageSource = ShakingHelper.GetBackgroundImage();
            backgroundImage.Stretch = Stretch.None;
            backgroundImage.AlignmentY = AlignmentY.Top;
            LayoutRoot.Background = backgroundImage;
        }

        private void ReviewApp_Click(object sender, EventArgs e)
        {
            reviewTask.Show();
        }

        private void OtherApps_Click(object sender, EventArgs e)
        {
            searchTask.SearchTerms = "Adam Speakman";
            searchTask.Show();
        }

        private void TwitterButton_Click(object sender, EventArgs e)
        {
            WebBrowserTask browserTask = new WebBrowserTask();
            browserTask.Uri = new Uri("https://mobile.twitter.com/whatsshakingnz", UriKind.Absolute);
            browserTask.Show();
        }

        private void EmailButton_Click(object sender, EventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "What's Shaking NZ App Feedback";
            emailComposeTask.To = "wp7-support@speakman.net.nz";

            emailComposeTask.Show();
        }

        private void UpgradeApp_Click(object sender, EventArgs e)
        {
            _marketPlaceDetailTask.Show();
        }
    }
}