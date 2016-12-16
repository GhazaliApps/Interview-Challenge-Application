using Interview_Challenge_Application.Helper;
using Interview_Challenge_Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
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
    public sealed partial class DetailsPage : Page
    {
        public Venue fsSelectedRestaurant= new Venue();
        public Result gSelectedRestaurant = new Result();
        public MapIcon MSelectedRestaurant = new MapIcon();
        private const string ApiKey = "AIzaSyDRgqi6JDcB8xXqUEE0bPuz8UPpH5yGlwI";

        public DetailsPage()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is MapIcon)
            {
                MSelectedRestaurant = e.Parameter as MapIcon;
                getRestaurantData(MSelectedRestaurant.Title);
                if (fsSelectedRestaurant != null)
                {
                    Rname.Text = fsSelectedRestaurant.name;
                    Rphone.Text = fsSelectedRestaurant.contact.formattedPhone.ToString();
                    Raddress.Text = fsSelectedRestaurant.location.address.ToString();
                }
                else
                {
                    GoogleRestaurantDetailsRoot GrestaurantDetails = await GetRestaurantsDetailsFromGoogle(gSelectedRestaurant.place_id);
                    Rname.Text = GrestaurantDetails.result.name;
                    Rphone.Text = GrestaurantDetails.result.formatted_phone_number;
                    Raddress.Text = GrestaurantDetails.result.formatted_address;
                }

            }

            //Recive Object as Venue from Four Square 
            else if (e.Parameter is Venue)
            {
                fsSelectedRestaurant = e.Parameter as Venue;
                if (fsSelectedRestaurant.name != null)
                    Rname.Text = fsSelectedRestaurant.name;
                if (fsSelectedRestaurant.contact.formattedPhone != null)
                    Rphone.Text = fsSelectedRestaurant.contact.formattedPhone.ToString();
                if(fsSelectedRestaurant.location.address!=null)
                    Raddress.Text = fsSelectedRestaurant.location.address.ToString();
                if (fsSelectedRestaurant.venueRatingBlacklisted != false)
                     Rrating.Text = "Good Reviews";
                else Rrating.Text = "Bad Reviews";
            }
            //Recive Object as Result from Google 
            else
            {
                gSelectedRestaurant = e.Parameter as Result;
                var restaurantData = await GetRestaurantsDetailsFromGoogle(gSelectedRestaurant.place_id.ToString());
                var RestaurantDetails = restaurantData.result;
                if(RestaurantDetails.name!=null)
                Rname.Text = RestaurantDetails.name.ToString();
                if (RestaurantDetails.formatted_phone_number != null)
                Rphone.Text = RestaurantDetails.formatted_phone_number.ToString();
                if (RestaurantDetails.formatted_address != null)
                    Raddress.Text = RestaurantDetails.formatted_address.ToString();
                if (RestaurantDetails.rating != 0)
                    Rrating.Text = "Good Reviews";
                else Rrating.Text = "Bad Reviews";

            }
        }

        private void Rphone_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Make a call by Restaurant Phone Number
          Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(Rphone.Text.ToString(),Rname.Text.ToString());
        }

        private async void Raddress_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Center on Device Location , Search for the address around this point.
            Geoposition POI= await LocationManager.GetPosition();
            var restaurantUri = new Uri(@"bingmaps:?+"+POI.Coordinate.Point+"&where="+Raddress.Text.ToString()+"&lvl=14");           
            // Launch the Windows Maps app
            var launcherOptions = new Windows.System.LauncherOptions();
            launcherOptions.TargetApplicationPackageFamilyName = "Microsoft.WindowsMaps_8wekyb3d8bbwe";
            var success = await Windows.System.Launcher.LaunchUriAsync(restaurantUri, launcherOptions);
        }

        private void getRestaurantData(string name)
        {
            List<Object> allS = (from x in App.Model.Fsrestaurants select (Object)x).ToList();
            allS.AddRange((from x in App.Model.Grestaurants select (Object)x).ToList());

            foreach (Object x in allS)
            {
                if (x is Venue)
                {
                    var r = x as Venue;
                    if (r.name == name)
                        fsSelectedRestaurant = r;
                }
               else if (x is Result)
                {
                    var r = x as Result;
                    if (r.name == name)
                        gSelectedRestaurant = r;
                }
            }

        }

        public static async Task<GoogleRestaurantDetailsRoot> GetRestaurantsDetailsFromGoogle(string placeId)
        {
            //Get Current Location 
            Geoposition currentLocation = await LocationManager.GetPosition();

            // Assemble the URL
            string url = String.Format("https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&key={1}",placeId, ApiKey);

            // Call out to FourSquare
            HttpClient http = new HttpClient();
            var response = await http.GetAsync(url);
            var jsonMessage = await response.Content.ReadAsStringAsync();
            // Response -> string / json -> deserialize
            var serializer = new DataContractJsonSerializer(typeof(GoogleRestaurantDetailsRoot));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonMessage));
            var result = (GoogleRestaurantDetailsRoot)serializer.ReadObject(ms);
            return result;
        }

    }
}

