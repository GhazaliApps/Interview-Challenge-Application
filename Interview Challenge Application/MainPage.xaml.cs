using Interview_Challenge_Application.Helper;
using Interview_Challenge_Application.Models;
using Interview_Challenge_Application.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
namespace Interview_Challenge_Application
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Foursquare
        private const string clientId = "MZ1NHLCK1GVCAUHANRXGDF31E52FEGYR2O20FZREZRJRT0PH";
        private const string clientSecret = "B5351JNMZHOSPFET4O20N4XYZFV1NGI3KVTMXDB20KDWAC2U";
        private const string categoryId = "4bf58dd8d48988d115941735";
        //Google Places
        private const string ApiKey = "AIzaSyDRgqi6JDcB8xXqUEE0bPuz8UPpH5yGlwI";
        private const string raduis = "5000";
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            bool isInternetConnected = NetworkInterface.GetIsNetworkAvailable();
            if (isInternetConnected)
            {
                var accessStatus = await Geolocator.RequestAccessAsync();
                if (accessStatus != GeolocationAccessStatus.Allowed)
                {
                    Frame.Navigate(typeof(NoLocationPage));
                }
                else
                {
                    LoadingProgressRing.IsActive = true;
                    LoadingProgressRing.Visibility = Visibility.Visible;
                    ListButton.IsEnabled = false;
                    MapButton.IsEnabled = false;
                    await GetAllRestaurantsDataFromFourSquare();
                    await GetAllRestaurantsDataFromGoogle();
                    await PopulateRestaurntsFromFoursquare();
                    await PopulateRestaurntsFromGoogle();
                    LoadingProgressRing.IsActive = false;
                    LoadingProgressRing.Visibility = Visibility.Collapsed;
                    if (App.Model.Grestaurants.Count == 0 && App.Model.Fsrestaurants.Count == 0)
                    {
                        MessageDialog NoRestaurantsMsg = new MessageDialog("Please try again later !", "No Restaurats around You !");
                        await NoRestaurantsMsg.ShowAsync();
                    }
                    else
                    { 
                    ListButton.IsEnabled = true;
                    MapButton.IsEnabled = true;
                    MyFrame.Navigate(typeof(ListViewPage));
                    }
                }
            }
            else
            {
                Frame.Navigate(typeof(NoInternetConnection));
            }
        }

        private void Map_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(MapView));
        }
        private void List_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(ListViewPage));
        }

        public static async Task<RootObject> GetAllRestaurantsDataFromFourSquare()
        {
            //Get Current Location 
            Geoposition currentLocation = await LocationManager.GetPosition();
            // Assemble the URL
            string url = String.Format("https://api.foursquare.com/v2/venues/search?categoryId={0}&radius={1}&client_id={2}&client_secret={3}&v=20130815&&ll={4},{5}", categoryId,raduis,clientId, clientSecret, currentLocation.Coordinate.Point.Position.Latitude, currentLocation.Coordinate.Point.Position.Longitude);

            // Call out to FourSquare
            HttpClient http = new HttpClient();
            var response = await http.GetAsync(url);
            var jsonMessage = await response.Content.ReadAsStringAsync();

            // Response -> string / json -> deserialize
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonMessage));
            var result = (RootObject)serializer.ReadObject(ms);
            return result;
        }
        public static async Task PopulateRestaurntsFromFoursquare()
        {
            var restaurantsData = await GetAllRestaurantsDataFromFourSquare();
            var allRestaurants = restaurantsData.response.venues;
            foreach (var restaurant in allRestaurants)
            {
                if (restaurant.name != null)
                    App.Model.Fsrestaurants.Add(restaurant);
            }
        }
        public static async Task<GoogleRestaurantsRoot> GetAllRestaurantsDataFromGoogle()
        {
            //Get Current Location 
            Geoposition currentLocation = await LocationManager.GetPosition();

            // Assemble the URL
            string url = String.Format("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={0},{1}&radius={2}&type=restaurant&keyword=cruise&key={3}", currentLocation.Coordinate.Point.Position.Latitude, currentLocation.Coordinate.Point.Position.Longitude,raduis,ApiKey);

            // Call out to FourSquare
            HttpClient http = new HttpClient();
            var response = await http.GetAsync(url);
            var jsonMessage = await response.Content.ReadAsStringAsync();
            // Response -> string / json -> deserialize
            var serializer = new DataContractJsonSerializer(typeof(GoogleRestaurantsRoot));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonMessage));
            var result = (GoogleRestaurantsRoot)serializer.ReadObject(ms);
            return result;
        }
        public static async Task PopulateRestaurntsFromGoogle()
        {
            var restaurantsData = await GetAllRestaurantsDataFromGoogle();

            var allRestaurants = restaurantsData.results;

            foreach (var restaurant in allRestaurants)
            {
                if (restaurant.name != null && restaurant.place_id!=null)
                   App.Model.Grestaurants.Add(restaurant);
            }
        }


    }
}
