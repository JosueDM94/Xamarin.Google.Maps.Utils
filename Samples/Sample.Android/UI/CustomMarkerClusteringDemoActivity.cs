using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;
using Android.Gms.Maps.Utils.Clustering.View;
using Android.Gms.Maps.Utils.UI;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Sample.Android.Utils;
using Person = Sample.Android.Models.Person;

namespace Sample.Android
{
    [Activity(Label = "CustomMarkerClusteringDemoActivity")]
    public class CustomMarkerClusteringDemoActivity : BaseDemoActivity, ClusterManager.IOnClusterClickListener, ClusterManager.IOnClusterItemClickListener, ClusterManager.IOnClusterInfoWindowClickListener, ClusterManager.IOnClusterItemInfoWindowClickListener
    {
        private ClusterManager mClusterManager;
        private Random mRandom = new Random(1984);

        /**
         * Draws profile photos inside markers (using IconGenerator).
         * When there are multiple people in the cluster, draw multiple photos (using MultiDrawable).
         */
        private class PersonRenderer : DefaultClusterRenderer
        {
            private Context context;
            private IconGenerator mIconGenerator;
            private IconGenerator mClusterIconGenerator;
            private ImageView mImageView;
            private ImageView mClusterImageView;
            private int mDimension;

            public PersonRenderer(CustomMarkerClusteringDemoActivity activity) :
                base(activity.ApplicationContext, activity.getMap(), activity.mClusterManager)
            {
                context = activity;
                mIconGenerator = new IconGenerator(activity.ApplicationContext);
                mClusterIconGenerator = new IconGenerator(activity.ApplicationContext);

                View multiProfile = LayoutInflater.From(activity).Inflate(Resource.Layout.multi_profile, null);
                mClusterIconGenerator.SetContentView(multiProfile);
                mClusterImageView = multiProfile.FindViewById<ImageView>(Resource.Id.image);

                mImageView = new ImageView(activity.ApplicationContext);
                mDimension = (int)activity.Resources.GetDimension(Resource.Dimension.custom_profile_image);
                mImageView.LayoutParameters = new ViewGroup.LayoutParams(mDimension, mDimension);
                int padding = (int)activity.Resources.GetDimension(Resource.Dimension.custom_profile_padding);
                mImageView.SetPadding(padding, padding, padding, padding);
                mIconGenerator.SetContentView(mImageView);
            }
            protected override void OnBeforeClusterItemRendered(Java.Lang.Object item, MarkerOptions markerOptions)
            {
                base.OnBeforeClusterItemRendered(item, markerOptions);
                // Draw a single person - show their profile photo and set the info window to show their name
                var person = item as Person;
                markerOptions.SetIcon(getItemIcon(person)).SetTitle(person.name);
            }

            //public void OnClusterItemUpdated(Person person, Marker marker)
            //{
            //    // Same implementation as onBeforeClusterItemRendered() (to update cached markers)
            //    marker.SetIcon(getItemIcon(person));
            //    marker.Title = person.Name;
            //}

            /**
             * Get a descriptor for a single person (i.e., a marker outside a cluster) from their
             * profile photo to be used for a marker icon
             *
             * @param person person to return an BitmapDescriptor for
             * @return the person's profile photo as a BitmapDescriptor
             */
            private BitmapDescriptor getItemIcon(Person person)
            {
                mImageView.SetImageResource(person.profilePhoto);
                Bitmap icon = mIconGenerator.MakeIcon();
                return BitmapDescriptorFactory.FromBitmap(icon);
            }

            protected override void OnBeforeClusterRendered(ICluster cluster, MarkerOptions markerOptions)
            {
                // Draw multiple people.
                // Note: this method runs on the UI thread. Don't spend too much time in here (like in this example).
                markerOptions.SetIcon(getClusterIcon(cluster));
            }

            //protected override void OnClusterUpdated(ICluster cluster, Marker marker)
            //{
            //    // Same implementation as onBeforeClusterRendered() (to update cached markers)
            //    marker.SetIcon(getClusterIcon(cluster));
            //}

            /**
             * Get a descriptor for multiple people (a cluster) to be used for a marker icon. Note: this
             * method runs on the UI thread. Don't spend too much time in here (like in this example).
             *
             * @param cluster cluster to draw a BitmapDescriptor for
             * @return a BitmapDescriptor representing a cluster
             */
            private BitmapDescriptor getClusterIcon(ICluster cluster)
            {
                List<Drawable> profilePhotos = new List<Drawable>(Math.Min(4, cluster.Size));
                int width = mDimension;
                int height = mDimension;

                foreach (Person p in cluster.Items)
                {
                    // Draw 4 at most.
                    if (profilePhotos.Count == 4) break;
#pragma warning disable CS0618 // Type or member is obsolete
                    Drawable drawable = context.Resources.GetDrawable(p.profilePhoto);
#pragma warning restore CS0618 // Type or member is obsolete
                    drawable.SetBounds(0, 0, width, height);
                    profilePhotos.Add(drawable);
                }
                MultiDrawable multiDrawable = new MultiDrawable(profilePhotos);
                multiDrawable.SetBounds(0, 0, width, height);

                mClusterImageView.SetImageDrawable(multiDrawable);
                Bitmap icon = mClusterIconGenerator.MakeIcon(cluster.Size.ToString());
                return BitmapDescriptorFactory.FromBitmap(icon);
            }
            
            protected override bool ShouldRenderAsCluster(ICluster cluster)
            {
                // Always render clusters.
                return cluster.Size > 1;
            }
        }

        public bool OnClusterClick(ICluster cluster)
        {
            // Show a toast with some info when the cluster is clicked.
            while (cluster.Items.GetEnumerator().MoveNext())
            {
                var person = cluster.Items.GetEnumerator().Current as Person;
                Toast.MakeText(this, cluster.Size + "(including " + person.name + ")", ToastLength.Short).Show();
            }

            // Zoom in the cluster. Need to create LatLngBounds and including all the cluster items
            // inside of bounds, then animate to center of the bounds.

            // Create the builder to collect all essential cluster items for the bounds.
            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            foreach (IClusterItem item in cluster.Items)
            {
                builder.Include(item.Position);
            }
            // Get the LatLngBounds
            LatLngBounds bounds = builder.Build();

            // Animate camera to the bounds
            try
            {
                getMap().AnimateCamera(CameraUpdateFactory.NewLatLngBounds(bounds, 100));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return true;
        }

        public void OnClusterInfoWindowClick(ICluster cluster)
        {
            // Does nothing, but you could go to a list of the users.
        }

        public bool OnClusterItemClick(Java.Lang.Object p0)
        {
            // Does nothing, but you could go into the user's profile page, for example.
            return false;
        }

        public void OnClusterItemInfoWindowClick(Java.Lang.Object p0)
        {
            // Does nothing, but you could go into the user's profile page, for example.
        }

        protected override void startDemo(bool isRestore)
        {
            if (!isRestore)
            {
                getMap().MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(51.503186, -0.126446), 9.5f));
            }

            mClusterManager = new ClusterManager(this, getMap());
            mClusterManager.Renderer = new PersonRenderer(this);
            getMap().SetOnCameraIdleListener(mClusterManager);
            getMap().SetOnMarkerClickListener(mClusterManager);
            getMap().SetOnInfoWindowClickListener(mClusterManager);
            mClusterManager.SetOnClusterClickListener(this);
            mClusterManager.SetOnClusterInfoWindowClickListener(this);
            mClusterManager.SetOnClusterItemClickListener(this);
            mClusterManager.SetOnClusterItemInfoWindowClickListener(this);

            addItems();
            mClusterManager.Cluster();
        }

        private void addItems()
        {
            // http://www.flickr.com/photos/sdasmarchives/5036248203/
            mClusterManager.AddItem(new Person(position(), "Walter", Resource.Drawable.walter));

            // http://www.flickr.com/photos/usnationalarchives/4726917149/
            mClusterManager.AddItem(new Person(position(), "Gran", Resource.Drawable.gran));

            // http://www.flickr.com/photos/nypl/3111525394/
            mClusterManager.AddItem(new Person(position(), "Ruth", Resource.Drawable.ruth));

            // http://www.flickr.com/photos/smithsonian/2887433330/
            mClusterManager.AddItem(new Person(position(), "Stefan", Resource.Drawable.stefan));

            // http://www.flickr.com/photos/library_of_congress/2179915182/
            mClusterManager.AddItem(new Person(position(), "Mechanic", Resource.Drawable.mechanic));

            // http://www.flickr.com/photos/nationalmediamuseum/7893552556/
            mClusterManager.AddItem(new Person(position(), "Yeats", Resource.Drawable.yeats));

            // http://www.flickr.com/photos/sdasmarchives/5036231225/
            mClusterManager.AddItem(new Person(position(), "John", Resource.Drawable.john));

            // http://www.flickr.com/photos/anmm_thecommons/7694202096/
            mClusterManager.AddItem(new Person(position(), "Trevor the Turtle", Resource.Drawable.turtle));

            // http://www.flickr.com/photos/usnationalarchives/4726892651/
            mClusterManager.AddItem(new Person(position(), "Teach", Resource.Drawable.teacher));
        }

        private LatLng position()
        {
            return new LatLng(random(51.6723432, 51.38494009999999), random(0.148271, -0.3514683));
        }

        private double random(double min, double max)
        {
            return mRandom.NextDouble() * (max - min) + min;
        }
    }
}
