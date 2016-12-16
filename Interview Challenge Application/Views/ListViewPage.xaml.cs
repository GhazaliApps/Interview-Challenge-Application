using Interview_Challenge_Application.Helper;
using Interview_Challenge_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class ListViewPage : Page
    {   
        public ListViewPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;

            List<Object> allS = (from x in App.Model.Fsrestaurants select (Object)x).ToList();
           allS.AddRange((from x in App.Model.Grestaurants select (Object)x).ToList());

            RestaurantsListView.ItemsSource = allS;
            MyProgressRing.IsActive = false;
            MyProgressRing.Visibility = Visibility.Collapsed;
        }

        private void RestaurantsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RestaurantsListView.SelectedIndex != -1)
            {
                if (RestaurantsListView.SelectedItem is Venue)
                {
                    var listitem = RestaurantsListView.SelectedItem as Venue;
                    Frame.Navigate(typeof(DetailsPage), listitem);
                }
                else
                {
                    var listitem = RestaurantsListView.SelectedItem as Result;
                    Frame.Navigate(typeof(DetailsPage), listitem);
                }
            }

        }
      }

    }

