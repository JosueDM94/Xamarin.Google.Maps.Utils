using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;

namespace Sample.Android.Models
{
    public class MyItem : Java.Lang.Object, IClusterItem
    {
        private LatLng mPosition;
        private string mTitle;
        private string mSnippet;

        public MyItem(double lat, double lng)
        {
            mPosition = new LatLng(lat, lng);
            mTitle = null;
            mSnippet = null;
        }

        public MyItem(double lat, double lng, string title, string snippet)
        {
            mPosition = new LatLng(lat, lng);
            mTitle = title;
            mSnippet = snippet;
        }

        public LatLng Position => mPosition;

        public string Snippet => mTitle;

        public string Title => mSnippet;

        /**
         * Set the title of the marker
         * @param title string to be set as title
         */
        public void SetTitle(string title)
        {
            mTitle = title;
        }

        /**
         * Set the description of the marker
         * @param snippet string to be set as snippet
         */
        public void SetSnippet(string snippet)
        {
            mSnippet = snippet;
        }
    }
}