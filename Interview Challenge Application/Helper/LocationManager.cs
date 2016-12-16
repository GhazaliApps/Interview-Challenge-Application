using Interview_Challenge_Application.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;

namespace Interview_Challenge_Application.Helper
{
    public class LocationManager
    {
        public async static Task<Geoposition> GetPosition()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            var geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
            var position = await geolocator.GetGeopositionAsync();
            return position;
        }

    }
}
