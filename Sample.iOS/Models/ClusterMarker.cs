using CoreLocation;
using Google.Maps;
using Google.Maps.Utils;

namespace Sample.iOS.Models
{
    public class ClusterMarker : Marker, IGMUClusterItem
    {
        public ClusterMarker(string Title, CLLocationCoordinate2D Position)
        {
            this.Title = Title;
            this.Position = Position;
        }
    }
}