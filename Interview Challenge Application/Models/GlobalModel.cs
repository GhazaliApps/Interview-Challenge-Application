using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview_Challenge_Application.Models
{
   public class GlobalModel
    {
        public ObservableCollection<Venue> Fsrestaurants { get; set; } = new ObservableCollection<Venue>();
        public ObservableCollection<Result> Grestaurants { get; set; } = new ObservableCollection<Result>();
       
    }
}
