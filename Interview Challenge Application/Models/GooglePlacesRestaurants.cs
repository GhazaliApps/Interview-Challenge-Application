using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview_Challenge_Application.Models
{
    public class GoogleLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }
    
    public class Geometry
    {
        public GoogleLocation location { get; set; }
    }
    public class Result
    {
        public Geometry geometry { get; set; }
        public string icon { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public string scope { get; set; }
        public List<string> types { get; set; }
        public string vicinity { get; set; }
        public double? rating { get; set; }
        public int? price_level { get; set; }
    }

    public class GoogleRestaurantsRoot
    {   public List<Result> results { get; set; }
        public string status { get; set; }
    }
}
