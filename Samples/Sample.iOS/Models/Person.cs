using CoreLocation;
using Foundation;
using Google.Maps.Utils;
using UIKit;

namespace Sample.iOS.Models
{
    public class Person : NSObject, IGMUClusterItem
    {
        private CLLocationCoordinate2D position;
        public string imageUrl { get; set; }
        public UIImage cacheImage { get; set; }

        public CLLocationCoordinate2D Position => position;

        public Person(CLLocationCoordinate2D position, string imageUrl)
        {            
            this.position = position;
            this.imageUrl = imageUrl;
        }
    }
}
