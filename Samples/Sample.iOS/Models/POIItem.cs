using CoreLocation;
using Foundation;
using Google.Maps.Utils;

namespace Sample.iOS.Models
{
    public class POIItem : NSObject, IGMUClusterItem
    {
        private CLLocationCoordinate2D position;
        public string name;

        public POIItem(CLLocationCoordinate2D position, string name)
        {
            this.name = name;
            this.position = position;
        }

        public CLLocationCoordinate2D Position => position;
    }
}