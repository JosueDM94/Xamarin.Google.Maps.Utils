using System;
using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.UI;
using Android.Graphics.Drawables;
using Android.Gms.Maps.Utils.Clustering;
using Android.Gms.Maps.Utils.Clustering.View;

using Sample.Droid.Utils;
using Sample.Droid.Models;
using Sample.Droid.Views.Base;

namespace Sample.Droid.Views.CustomMarkerClustering
{
    [Activity(Label = "CustomMarkerClusteringActivity")]
    public class CustomMarkerClusteringActivity : BaseActivity, ClusterManager.IOnClusterClickListener,ClusterManager.IOnClusterItemClickListener,ClusterManager.IOnClusterInfoWindowClickListener, ClusterManager.IOnClusterItemInfoWindowClickListener
    {
        private ClusterManager clusterManager;
        private Random random = new Random(1984);

        protected override void StartMap()
        {
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(51.503186, -0.126446), 9.5f));
            clusterManager = new ClusterManager(this, googleMap);
            clusterManager.Renderer = new PersonRenderer(this,googleMap,clusterManager);
            googleMap.SetOnCameraIdleListener(clusterManager);
            googleMap.SetOnMarkerClickListener(clusterManager);
            googleMap.SetOnInfoWindowClickListener(clusterManager);
            clusterManager.SetOnClusterClickListener(this);
            clusterManager.SetOnClusterItemClickListener(this);
            clusterManager.SetOnClusterInfoWindowClickListener(this);
            clusterManager.SetOnClusterItemInfoWindowClickListener(this);

            AddItems();
            clusterManager.Cluster();
        }

        private void AddItems()
        {
            // http://www.flickr.com/photos/sdasmarchives/5036248203/
            clusterManager.AddItem(new Person(Position(), "Walter", Resource.Drawable.walter));

            // http://www.flickr.com/photos/usnationalarchives/4726917149/
            clusterManager.AddItem(new Person(Position(), "Gran", Resource.Drawable.gran));

            // http://www.flickr.com/photos/nypl/3111525394/
            clusterManager.AddItem(new Person(Position(), "Ruth", Resource.Drawable.ruth));

            // http://www.flickr.com/photos/smithsonian/2887433330/
            clusterManager.AddItem(new Person(Position(), "Stefan", Resource.Drawable.stefan));

            // http://www.flickr.com/photos/library_of_congress/2179915182/
            clusterManager.AddItem(new Person(Position(), "Mechanic", Resource.Drawable.mechanic));

            // http://www.flickr.com/photos/nationalmediamuseum/7893552556/
            clusterManager.AddItem(new Person(Position(), "Yeats", Resource.Drawable.yeats));

            // http://www.flickr.com/photos/sdasmarchives/5036231225/
            clusterManager.AddItem(new Person(Position(), "John", Resource.Drawable.john));

            // http://www.flickr.com/photos/anmm_thecommons/7694202096/
            clusterManager.AddItem(new Person(Position(), "Trevor the Turtle", Resource.Drawable.turtle));

            // http://www.flickr.com/photos/usnationalarchives/4726892651/
            clusterManager.AddItem(new Person(Position(), "Teach", Resource.Drawable.teacher));
        }

        private LatLng Position()
        {
            return new LatLng(Random(51.6723432, 51.38494009999999), Random(0.148271, -0.3514683));
        }

        private double Random(double min, double max)
        {
            return random.NextDouble() * (max - min) + min;
        }

        public bool OnClusterClick(ICluster p0)
        {
            //foreach (var item in p0.Items)
            //{
            //    var person = item as Person;
            //    Toast.MakeText(this, p0.Size + "(including " + person.Name + ")", ToastLength.Short).Show();
            //}
            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            foreach(var item in p0.Items)                            
                builder.Include((item as Person).Position);
            LatLngBounds bounds = builder.Build();
            try{
                googleMap.AnimateCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 100));
            }catch(System.Exception ex){
                Console.WriteLine(ex.StackTrace);
            }

            return true;
        }

        public void OnClusterInfoWindowClick(ICluster p0)
        {

        }

        public bool OnClusterItemClick(Java.Lang.Object p0)
        {
            return false;
        }

        public void OnClusterItemInfoWindowClick(Java.Lang.Object p0)
        {

        }

        private class PersonRenderer : DefaultClusterRenderer
        {
            private int dimension;
            private Context context;
            private ImageView imageView;
            private ImageView clusterImageView;
            private IconGenerator iconGenerator;
            private IconGenerator clusterIconGenerator;           

            public PersonRenderer(Context context,GoogleMap googleMap,ClusterManager clusterManager) 
                :base(context,googleMap,clusterManager)
            {
                this.context = context;
                iconGenerator = new IconGenerator(context);
                clusterIconGenerator = new IconGenerator(context);

                View multiProfile = LayoutInflater.From(context).Inflate(Resource.Layout.MultiProfileView,null);
                clusterIconGenerator.SetContentView(multiProfile);
                clusterImageView = multiProfile.FindViewById<ImageView>(Resource.Id.image);
                imageView = new ImageView(context);
                dimension = (int)context.Resources.GetDimension(Resource.Dimension.custom_profile_image);
                imageView.LayoutParameters = new ViewGroup.LayoutParams(dimension,dimension);
                int padding = (int)context.Resources.GetDimension(Resource.Dimension.custom_profile_padding);
                imageView.SetPadding(padding,padding,padding,padding);
                iconGenerator.SetContentView(imageView);
            }

            protected override void OnBeforeClusterItemRendered(Java.Lang.Object p0, MarkerOptions p1)
            {
                // Draw a single person.
                // Set the info window to show their name.
                var person = p0 as Person;
                imageView.SetImageResource(person.Photo);
                Bitmap icon = iconGenerator.MakeIcon();
                p1.SetIcon(BitmapDescriptorFactory.FromBitmap(icon)).SetTitle(person.Name);
            }

            protected override void OnBeforeClusterRendered(ICluster p0, MarkerOptions p1)
            {
                List<Drawable> photos = new List<Drawable>(System.Math.Min(4,p0.Size));
                int width = dimension;
                int height = dimension;

                foreach(var person in p0.Items)
                {
                    if (photos.Count == 4)
                        return;
                    var p = person as Person;
                    Drawable drawable = context.GetDrawable(p.Photo);
                    drawable.SetBounds(0,0,width,height);
                    photos.Add(drawable);
                }

                MultiDrawable multiDrawable = new MultiDrawable(photos);
                multiDrawable.SetBounds(0, 0, width, height);
                clusterImageView.SetImageDrawable(multiDrawable);
                Bitmap icon = clusterIconGenerator.MakeIcon(p0.Size.ToString());
                p1.SetIcon(BitmapDescriptorFactory.FromBitmap(icon));
            }

            protected override bool ShouldRenderAsCluster(ICluster p0)
            {
                return p0.Size > 1;
            }
        }
    }
}