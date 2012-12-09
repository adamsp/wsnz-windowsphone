using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Shell;
using WhatsShakingNZ.Localization;
using WhatsShakingNZ.GeonetHelper;

namespace WhatsShakingNZ
{
    public partial class MainPage : WhatsShakingBasePage
    {
        private enum ButtonNames { RefreshButton = 0, MapButton };
        
        public MainPage()
        {
            InitializeComponent();
            InitializeApplicationBar();
            this.DataContext = QuakeContainer;
            SetBackgroundImage();
            LittleWatson.CheckForPreviousException();
            if (AppSettingsForPage.ShowLiveTileSetting)
                ScheduledTaskHelper.Update();
        }

        private void SetBackgroundImage()
        {
            ImageBrush backgroundImage = new ImageBrush();
            backgroundImage.ImageSource = ShakingHelper.GetBackgroundImage();
            LayoutRoot.Background = backgroundImage;
        }

        private void InitializeApplicationBar()
        {
            /***
             * Make sure these buttons are added in the same order as the button names in the 
             * enumeration above. Otherwise we don't know which button is where - see GetQuakes().
             **/
            List<ApplicationBarIconButton> buttons = new List<ApplicationBarIconButton>();
            
            ApplicationBarIconButton refreshButton = new ApplicationBarIconButton();
            refreshButton.Text = AppResources.AppBarRefreshButtonText;
            refreshButton.IconUri = new Uri("/Icons/appbar.refresh.rest.png", UriKind.Relative);
            refreshButton.Click += RefreshRecentButton_Click;
            
            ApplicationBarIconButton mapViewButton = new ApplicationBarIconButton();
            mapViewButton.Text = AppResources.AppBarMapViewButtonText;
            mapViewButton.IconUri = new Uri("/Icons/appbar.map.png", UriKind.Relative);
            mapViewButton.Click += MapPageButton_Click;
            
            buttons.Add(refreshButton);
            buttons.Add(mapViewButton);

            base.InitializeApplicationBar(buttons);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != System.Windows.Navigation.NavigationMode.Back)
                DownloadNewQuakes();
            else
                RefreshViews();
        }

        protected override void StartGetQuakes()
        {
            /**
             * Can't put this in the base class because the refresh button has no identifier - so we can't
             * just find it in the collection. Ugh.
             * */
            (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = false;
            customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Visible;
            customIndeterminateProgressBar.IsIndeterminate = true;
        }

        protected override void GetQuakesFinished()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = true;
                customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                customIndeterminateProgressBar.IsIndeterminate = false;
            });
        }

        private void MapPageButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapPage.xaml", UriKind.Relative));
        }

        protected override void QuakeItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ContentPanel.SelectedItem != null)
            {
                int selectedIndex = QuakeContainer.Quakes.IndexOf(ContentPanel.SelectedItem as Earthquake);
                ContentPanel.SelectedItem = null;
                NavigateToQuakePage(selectedIndex);
            }
        }

    }
}