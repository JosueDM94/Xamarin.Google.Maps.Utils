using System;
using CoreLocation;
using Google.Maps;
using Google.Maps.Utils;
using UIKit;

namespace Sample.iOS.Models
{
    public class Person : Marker, IGMUClusterItem
    {        
        public string imageUrl { get; set; }
        public UIImage cacheImage { get; set; }

        public Person(CLLocationCoordinate2D Position,string imageUrl)
        {            
            this.Position = Position;
            this.imageUrl = imageUrl;
        }
    }
}
