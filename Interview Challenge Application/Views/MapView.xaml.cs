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
    public sealed partial class MapView : Page
    {
        public MapView()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MyProgressRing.IsActive = true;
            MyProgressRing.Visibility = Visibility.Visible;
            await DrawFSRestaurantsOnMap();
            await DrawFSRestaurantsOnMap();
            MyProgressRing.IsActive = false;
            MyProgressRing.Visibility = Visibility.Collapsed;

        }

        private async Task DrawGoogleRestaurantsOnMap()
        {
            
            var allRestaurantsfromGoogle = App.Model.Grestaurants;
            Geoposition snPosition = await LocationManager.GetPosition();

            foreach (var restaurant in allRestaurantsfromGoogle)
            {
                // Create a MapIcon.
                MapIcon restaurantOnMap = new MapIcon();
                restaurantOnMap.Location = new Geopoint(new BasicGeoposition
                {
                    Latitude = restaurant.geometry.location.lat,
                    Longitude = restaurant.geometry.location.lng,
                });
                restaurantOnMap.NormalizedAnchorPoint = new Point(0.5, 1.0);
                restaurantOnMap.Title = restaurant.name;
                restaurantOnMap.ZIndex = 0;

                // Add the MapIcon to the map.
                restaurantsMap.MapElements.Add(restaurantOnMap);

                // Center the map over the POI.
                restaurantsMap.Center = snPosition.Coordinate.Point;
                restaurantsMap.ZoomLevel = 14;
            }

        }
        private async Task DrawFSRestaurantsOnMap()
        {
            
            var allRestaurantsfromFS = App.Model.Fsrestaurants;

            Geoposition snPosition = await LocationManager.GetPosition();

            foreach (var restaurant in allRestaurantsfromFS)
            {

                // Create a MapIcon.
                MapIcon restaurantOnMap = new MapIcon();
                restaurantOnMap.Location = new Geopoint(new BasicGeoposition
                {
                    Latitude = restaurant.location.lat,
                    Longitude = restaurant.location.lng,
                });
                restaurantOnMap.NormalizedAnchorPoint = new Point(0.5, 1.0);
                restaurantOnMap.Title = restaurant.name;
                restaurantOnMap.ZIndex = 0;

                // Add the MapIcon to the map.
                restaurantsMap.MapElements.Add(restaurantOnMap);

                // Center the map over the POI.
                restaurantsMap.Center = snPosition.Coordinate.Point;
                restaurantsMap.ZoomLevel = 14;
            }

        }

        private void restaurantsMap_MapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            MapIcon myClickedIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            Frame.Navigate(typeof(DetailsPage), myClickedIcon);

        }
    }
}
