using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Shell;

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
            refreshButton.Text = "refresh";
            refreshButton.IconUri = new Uri("/Icons/appbar.refresh.rest.png", UriKind.Relative);
            refreshButton.Click += RefreshRecentButton_Click;
            
            ApplicationBarIconButton mapViewButton = new ApplicationBarIconButton();
            mapViewButton.Text = "map view";
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
                GetQuakes();
            else
                QuakeContainer.RefreshViews();
        }

        protected override void GetQuakes()
        {
            /**
             * Can't put this in the base class because the refresh button has no identifier - so we can't
             * just find it in the collection. Ugh.
             * */
            (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = false;
            customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Visible;
            customIndeterminateProgressBar.IsIndeterminate = true;
            QuakeContainer.DownloadNewQuakes();
        }

        public override void QuakesUpdatedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName == "Quakes")
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    (ApplicationBar.Buttons[(int)ButtonNames.RefreshButton] as ApplicationBarIconButton).IsEnabled = true;
                    customIndeterminateProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                    customIndeterminateProgressBar.IsIndeterminate = false;
                    if (QuakeContainer.Quakes.Count == 0)
                    {
                        ShowNoConnectivityToast();
                    }
                });
            }
        }

        private void MapPageButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapPage.xaml", UriKind.Relative));
        }

        protected override void QuakeItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (ContentPanel.SelectedIndex >= 0)
            {
                int selectedIndex = ContentPanel.SelectedIndex;
                ContentPanel.SelectedItem = null;
                NavigateToQuakePage(selectedIndex);
            }
        }

    }
}