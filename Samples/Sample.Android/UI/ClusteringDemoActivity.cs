using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;
using Android.Views;
using Android.Widget;
using Org.Json;
using Sample.Android.Models;
using Sample.Android.Utils;

namespace Sample.Android
{
    [Activity(Label = "ClusteringDemoActivity")]
    public class ClusteringDemoActivity : BaseDemoActivity, GoogleMap.IInfoWindowAdapter, GoogleMap.IOnInfoWindowClickListener
    {
        private ClusterManager mClusterManager;

        protected override void startDemo(bool isRestore)
        {
            if (!isRestore)
            {
                getMap().MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(51.503186, -0.126446), 10));
            }

            mClusterManager = new ClusterManager(this, getMap());
            getMap().SetOnCameraIdleListener(mClusterManager);

            // Add a custom InfoWindowAdapter by setting it to the MarkerManager.Collection object from
            // ClusterManager rather than from GoogleMap.setInfoWindowAdapter
            mClusterManager.MarkerCollection.SetOnInfoWindowAdapter(this);
            mClusterManager.MarkerCollection.SetOnInfoWindowClickListener(this);
                

            try
            {
                readItems();
            }
            catch (JSONException)
            {
                Toast.MakeText(this, "Problem reading list of markers.", ToastLength.Long).Show();
            }
        }

        private void readItems() 
        {
            Stream inputStream = Resources.OpenRawResource(Resource.Raw.radar_search);
            List<MyItem> items = new MyItemReader().read(inputStream);
            mClusterManager.AddItems(items);
        }

        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            LayoutInflater inflater = LayoutInflater.From(this);
            View view = inflater.Inflate(Resource.Layout.custom_info_window, null);
            TextView textView = view.FindViewById<TextView>(Resource.Id.textViewTitle);
            String text = (marker.Title != null) ? marker.Title : "Cluster Item";
            textView.Text = text;
            return view;
        }

        public void OnInfoWindowClick(Marker marker)
        {
            Toast.MakeText(this, "Info window clicked.", ToastLength.Long).Show();
        }
    }
}
