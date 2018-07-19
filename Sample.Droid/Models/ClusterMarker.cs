using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;

namespace Sample.Droid.Models
{
    public class ClusterMarker : Java.Lang.Object ,IClusterItem
    {
        public LatLng Position { get; set; }

        public string Snippet { get; set; }

        public string Title { get; set; }

        public ClusterMarker()
        {
        }

        public ClusterMarker(LatLng position)
        {
            Position = position;
        }

        public ClusterMarker(double lat, double lng)
        {
            Position = new LatLng(lat, lng);
            Title = null;
            Snippet = null;
        }
    }
}