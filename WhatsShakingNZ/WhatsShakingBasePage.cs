using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhatsShakingNZ.GeonetHelper;
using WhatsShakingNZ.Settings;

namespace WhatsShakingNZ
{
    /// <summary>
    /// Base class for the List and Map view pages. Contains shared functionality.
    /// </summary>
    public abstract partial class WhatsShakingBasePage : PhoneApplicationPage
    {
        private AppSettings _appSettings;
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
            settingsMenuItem.Text = "settings";
            settingsMenuItem.Click += SettingsPageMenuItem_Click;

            ApplicationBarMenuItem aboutMenuItem = new ApplicationBarMenuItem();
            aboutMenuItem.Text = "about";
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
        
        protected abstract void GetQuakes();

        private EarthquakeContainer _quakeContainer;
        protected EarthquakeContainer QuakeContainer
        {
            get
            {
                if (null == _quakeContainer)
                    _quakeContainer = (Application.Current as App).EarthquakeContainer;
                return _quakeContainer;
            }
        }

        public abstract void QuakesUpdatedEventHandler(object sender, PropertyChangedEventArgs e);

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
            GetQuakes();
        }

        protected abstract void QuakeItem_Tap(object sender, System.Windows.Input.GestureEventArgs e);

        protected void NavigateToQuakePage(int selectedIndex)
        {
            string navUri = string.Format("/QuakeDisplayPage.xaml?selectedItem={0}", selectedIndex);
            NavigationService.Navigate(new Uri(navUri, UriKind.Relative));
        }

        protected void ShowNoConnectivityToast()
        {
            ToastPrompt toast = new ToastPrompt()
            {
                TextOrientation = System.Windows.Controls.Orientation.Vertical,
                Title = "problem retrieving quakes",
                Message = "please check your data connection is working"
            };
            toast.Show();
        }
    }
}
