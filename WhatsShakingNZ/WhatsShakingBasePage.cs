using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhatsShakingNZ.GeonetHelper;
using WhatsShakingNZ.Settings;
using WhatsShakingNZ.Localization;

namespace WhatsShakingNZ
{
    /// <summary>
    /// Base class for the List and Map view pages. Contains shared functionality.
    /// </summary>
    public abstract partial class WhatsShakingBasePage : PhoneApplicationPage
    {
        /// <summary>
        /// This method is called when an item in the subclasses set of quakes is tapped.
        /// It should pass the correct index (relative to the QuakeContainer.Quakes
        /// collection) to the NavigateToQuakePage method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void QuakeItem_Tap(object sender, System.Windows.Input.GestureEventArgs e);

        /// <summary>
        /// Used so the page can do any page-specific modifications (such as disabling the 
        /// Refresh button) before the QuakeContainer.DownloadNewQuakes() method gets called.
        /// </summary>
        protected abstract void StartGetQuakes();

        /// <summary>
        /// Used so the page can do any page-specific modifications (such as enabling the 
        /// Refresh button) after the QuakeContainer.DownloadNewQuakes() method returns.
        /// </summary>
        protected abstract void GetQuakesFinished();

        /// <summary>
        /// Makes the calls required to set up the pages for downloading new quakes, and
        /// then starts the download.
        /// </summary>
        protected void DownloadNewQuakes()
        {
            StartGetQuakes();
            QuakeContainer.DownloadNewQuakes();
        }

        /// <summary>
        /// Does not download new quakes but fires the "updated" event, useful when
        /// settings may have changed.
        /// </summary>
        protected void RefreshViews()
        {
            QuakeContainer.RefreshViews();
        }

        /// <summary>
        /// This will be attached to the QuakeContainer.PropertyChanged event automatically
        /// for you in the OnNavigatedTo method, and removed again in the OnNavigatedFrom.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void QuakesUpdatedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName == EarthquakeContainer.QuakesUpdatedEventKey)
            {
                GetQuakesFinished();
                if (!ShouldUpdateQuakesDisplay()) return;
                if (QuakeContainer.Quakes.Count == 0)
                {
                    ShowRestrictiveSettingsToast();
                }
            }
        }

        /// <summary>
        /// Will display a toast informing the user that something went wrong with the
        /// Geonet request, if something went wrong.
        /// </summary>
        /// <returns>true if nothing went wrong and you should update your display,
        /// or false if something did go wrong and you should leave old quakes there.</returns>
        protected bool ShouldUpdateQuakesDisplay()
        {
            string toastMessage;
            switch(QuakeContainer.Status)
            {
                case GeonetSuccessStatus.Success:
                    return true;
                case GeonetSuccessStatus.BadGeonetData:
                    toastMessage = AppResources.GeonetProblemBadGeonetData;
                    break;
                case GeonetSuccessStatus.NetworkFailure:
                    toastMessage = AppResources.GeonetProblemNetworkFailure;
                    break;
                case GeonetSuccessStatus.NoGeonetData:
                    toastMessage = AppResources.GeonetProblemNoGeonetData;
                    break;
                default:
                    toastMessage = AppResources.GeonetProblemDefault;
                    break;
            }

            ToastPrompt toast = new ToastPrompt()
            {
                TextOrientation = System.Windows.Controls.Orientation.Vertical,
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Title = AppResources.GeonetProblemToastTitle,
                Message = toastMessage
            };
            toast.Show();
            return false;
        }

        private EarthquakeContainer _quakeContainer;
        /// <summary>
        /// Used to download new quakes, update your display when navigated to, etc.
        /// </summary>
        protected EarthquakeContainer QuakeContainer
        {
            get
            {
                if (null == _quakeContainer)
                    _quakeContainer = (Application.Current as App).EarthquakeContainer;
                return _quakeContainer;
            }
        }

        private AppSettings _appSettings;
        /// <summary>
        /// Used to access the application settings.
        /// </summary>
        protected AppSettings AppSettingsForPage
        {
            get
            {
                if (null == _appSettings)
                    _appSettings = new AppSettings();
                return _appSettings;
            }
        }
        /// <summary>
        /// Initializes the application bar with default menu items.
        /// </summary>
        internal void InitializeApplicationBarDefaults()
        {
            ApplicationBarMenuItem settingsMenuItem = new ApplicationBarMenuItem();
            settingsMenuItem.Text = AppResources.AppBarSettingsMenuItemText;
            settingsMenuItem.Click += SettingsPageMenuItem_Click;

            ApplicationBarMenuItem aboutMenuItem = new ApplicationBarMenuItem();
            aboutMenuItem.Text = AppResources.AppBarAboutMenuItemText;
            aboutMenuItem.Click += AboutPageMenuItem_Click;

            ApplicationBar.MenuItems.Add(settingsMenuItem);
            ApplicationBar.MenuItems.Add(aboutMenuItem);
        }

        /// <summary>
        /// Initializes the application bar with the supplied buttons and menu items, as well as the defaults.
        /// </summary>
        /// <param name="buttons"></param>
        /// <param name="menuItems"></param>
        protected virtual void InitializeApplicationBar(List<ApplicationBarIconButton> buttons, List<ApplicationBarMenuItem> menuItems)
        {
            if (null != buttons)
                foreach (var button in buttons) ApplicationBar.Buttons.Add(button);
            if (null != menuItems)
                foreach (var menuItem in menuItems) ApplicationBar.MenuItems.Add(menuItem);
            InitializeApplicationBarDefaults();
        }

        /// <summary>
        /// Initializes the application bar with the supplied menu items, as well as the defaults.
        /// </summary>
        /// <param name="menuItems"></param>
        protected virtual void InitializeApplicationBar(List<ApplicationBarMenuItem> menuItems)
        {
            InitializeApplicationBar(null, menuItems);
        }

        /// <summary>
        /// Initializes the application bar with the supplied buttons, as well as the default menu items.
        /// </summary>
        /// <param name="buttons"></param>
        protected virtual void InitializeApplicationBar(List<ApplicationBarIconButton> buttons)
        {
            InitializeApplicationBar(buttons, null);
        }
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            QuakeContainer.PropertyChanged += QuakesUpdatedEventHandler;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            QuakeContainer.PropertyChanged -= QuakesUpdatedEventHandler;
            base.OnNavigatedFrom(e);
        }

        protected void SettingsPageMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        protected void AboutPageMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        protected void RefreshRecentButton_Click(object sender, EventArgs e)
        {
            DownloadNewQuakes();
        }

        protected void NavigateToQuakePage(int selectedIndex)
        {
            string navUri = string.Format("/QuakeDisplayPage.xaml?selectedItem={0}", selectedIndex);
            NavigationService.Navigate(new Uri(navUri, UriKind.Relative));
        }

        protected void ShowRestrictiveSettingsToast()
        {
            ToastPrompt toast = new ToastPrompt()
            {
                TextOrientation = System.Windows.Controls.Orientation.Vertical,
                TextWrapping = System.Windows.TextWrapping.Wrap,
                Title = AppResources.BasePageRestrictiveSettingsToastTitle,
                Message = AppResources.BasePageRestrictiveSettingsToastMessage
            };
            toast.Show();
        }
    }
}
