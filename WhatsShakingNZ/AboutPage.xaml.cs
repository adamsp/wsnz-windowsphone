using System;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

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
            PopulateAboutPage();
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

        private void PopulateAboutPage()
        {
            AboutAppTextBlock.Text = "What's Shaking NZ? uses the Geonet.org.nz data feed "
            + "to provide you with up to date information about the latest earthquakes around New Zealand.\n"
            + "Please tweet or email us with any problems you find or suggestions for features you'd like "
            + "to see in the next version.\n";
            Update12TextBlock.Text = "Geonet have recently changed the way they provide their data feed, and "
            + "we can now only retrieve the latest 30 quakes. Previously we could retrieve the latest N days "
            + "of quakes - this was limited to a maximum of 3 days within the app. However, quakes are now "
            + "reported on significantly faster.\n"
            + "Now you can specify the maximum number of quakes you would like displayed - from 10 to 30. In "
            + "some cases you may get less than your specified maximum, if your \"minimum magnitude to display\" "
            + "value is set high enough.\n"
            + "Modified the layout of this screen.\n"
            + "Fixed a bug where the application would occasionally crash on the Map view page.\n";
            Update11TextBlock.Text = "Fixed an issue where sometimes the application would crash when the "
            + "settings page was accessed too quickly.\n"
            + "Modified the magnitude and depth values to display with the correct number of decimal places, "
            + "as specified on the Geonet website.";
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