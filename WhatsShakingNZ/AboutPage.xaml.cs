using System;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using WhatsShakingNZ.Localization;
using Microsoft.Phone.Shell;

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
            InitializeApplicationBar();
            ImageBrush backgroundImage = new ImageBrush();
            backgroundImage.ImageSource = ShakingHelper.GetBackgroundImage();
            backgroundImage.Stretch = Stretch.None;
            backgroundImage.AlignmentY = AlignmentY.Top;
            LayoutRoot.Background = backgroundImage;
        }

        private void InitializeApplicationBar()
        {
            /**
             * Because the app bar isn't a Silverlight control (wtf) we can't use binding in it,
             * so if we want localisation we have to create all these in code. Sigh.
             **/
            ApplicationBarIconButton twitterButton = new ApplicationBarIconButton();
            twitterButton.Text = AppResources.AboutPageAppBarTwitterText;
            twitterButton.IconUri = new Uri("/Icons/appbar.twitter.bird.png", UriKind.Relative);
            twitterButton.Click += TwitterButton_Click;
            ApplicationBar.Buttons.Add(twitterButton);

            ApplicationBarIconButton emailButton = new ApplicationBarIconButton();
            emailButton.Text = AppResources.AboutPageAppBarEmailText;
            emailButton.IconUri = new Uri("/Icons/appbar.feature.email.rest.png", UriKind.Relative);
            emailButton.Click += EmailButton_Click;
            ApplicationBar.Buttons.Add(emailButton);

            ApplicationBarIconButton otherAppsButton = new ApplicationBarIconButton();
            otherAppsButton.Text = AppResources.AboutPageAppBarOtherAppsText;
            otherAppsButton.IconUri = new Uri("/Icons/appbar.marketplace.png", UriKind.Relative);
            otherAppsButton.Click += OtherApps_Click;
            ApplicationBar.Buttons.Add(otherAppsButton);

            ApplicationBarIconButton reviewAppButton = new ApplicationBarIconButton();
            reviewAppButton.Text = AppResources.AboutPageAppBarReviewAppText;
            reviewAppButton.IconUri = new Uri("/Icons/appbar.favs.rest.png", UriKind.Relative);
            reviewAppButton.Click += ReviewApp_Click;
            ApplicationBar.Buttons.Add(reviewAppButton);

            ApplicationBarMenuItem websiteMenuItem = new ApplicationBarMenuItem();
            websiteMenuItem.Text = AppResources.AboutPageAppBarWebsiteText;
            websiteMenuItem.Click += WebsiteMenuItem_Click;
            ApplicationBar.MenuItems.Add(websiteMenuItem);
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

        private void WebsiteMenuItem_Click(object sender, EventArgs e)
        {
            WebBrowserTask browserTask = new WebBrowserTask();
            browserTask.Uri = new Uri("http://www.whatsshaking.co.nz", UriKind.Absolute);
            browserTask.Show();
        }

        private void UpgradeApp_Click(object sender, EventArgs e)
        {
            _marketPlaceDetailTask.Show();
        }
    }
}