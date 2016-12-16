using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview_Challenge_Application.Models
{
   public class Contact
    {
        public string phone { get; set; }
        public string formattedPhone { get; set; }
        public string twitter { get; set; }
        public string facebook { get; set; }
        public string facebookUsername { get; set; }
        public string facebookName { get; set; }
    }

    public class LabeledLatLng
    {
        public string label { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Location
    {
        public string address { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public List<LabeledLatLng> labeledLatLngs { get; set; }
        public int distance { get; set; }
        public string cc { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public List<string> formattedAddress { get; set; }
        public string crossStreet { get; set; }
        public string postalCode { get; set; }
    }

    public class Stats
    {
        public int checkinsCount { get; set; }
        public int usersCount { get; set; }
        public int tipCount { get; set; }
    }

    public class VenuePage
    {
        public string id { get; set; }
    }

    public class Venue
    {
        public string id { get; set; }
        public string name { get; set; }
        public Contact contact { get; set; }
        public Location location { get; set; }
        public List<object> categories { get; set; }
        public bool verified { get; set; }
        public Stats stats { get; set; }
        public string referralId { get; set; }
        public List<object> venueChains { get; set; }
        public bool hasPerk { get; set; }
        public string url { get; set; }
        public VenuePage venuePage { get; set; }
        public string storeId { get; set; }
        public bool? venueRatingBlacklisted { get; set; }
    }

    public class Response
    {
        public List<Venue> venues { get; set; }
    }

    public class RootObject
    {
        public Response response { get; set; }
    }
}
