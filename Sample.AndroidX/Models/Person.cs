using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;

namespace Sample.AndroidX.Models
{
    public class Person : Java.Lang.Object, IClusterItem
    {
        public string name;
        public int profilePhoto;
        private LatLng mPosition;

        public Person(LatLng position, string name, int pictureResource)
        {
            this.name = name;
            profilePhoto = pictureResource;
            mPosition = position;
        }

        public LatLng Position => mPosition;

        public string Snippet => null;

        public string Title => null;
    }
}
