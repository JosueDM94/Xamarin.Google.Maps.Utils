using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;

namespace Sample.Droid.Models
{
    public class Person : Java.Lang.Object ,IClusterItem
    {
        public LatLng Position { get; set; }

        public string Snippet { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public int Photo { get; set; }

        public Person(LatLng position)
        {
            Position = position;
        }

        public Person(LatLng position, string name, int photo) : this(position)
        {
            Name = name;
            Photo = photo;
        }
    }
}