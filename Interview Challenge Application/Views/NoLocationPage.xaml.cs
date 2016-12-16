using Interview_Challenge_Application.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Interview_Challenge_Application.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoLocationPage : Page
    {
        public NoLocationPage()
        {
            this.InitializeComponent();
        }
        async private void RefreshClicked(object sender, RoutedEventArgs e)
        {
            var access_status = await Geolocator.RequestAccessAsync();
            switch (access_status)
            {
                case GeolocationAccessStatus.Allowed:
                    Frame.Navigate(typeof(MainPage));
                    break;
                case GeolocationAccessStatus.Denied:
                    break;
                case GeolocationAccessStatus.Unspecified:
                    break;
            }
        }
    }
}
