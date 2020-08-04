using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;
using Android.Gms.Maps.Utils.Clustering.Algorithm;
using Android.Util;
using Android.Widget;
using Org.Json;
using Sample.AndroidX.Models;
using Sample.AndroidX.Utils;

namespace Sample.AndroidX
{
    [Activity(Label = "VisibleClusteringDemoActivity")]
    public class VisibleClusteringDemoActivity : BaseDemoActivity
    {
        private ClusterManager mClusterManager;

        protected override void startDemo(bool isRestore)
        {
            DisplayMetrics metrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(metrics);

            if (!isRestore)
            {
                getMap().MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(51.503186, -0.126446), 10));
            }

            mClusterManager = new ClusterManager(this, getMap());
            mClusterManager.Algorithm = new NonHierarchicalDistanceBasedAlgorithm();//new NonHierarchicalViewBasedAlgorithm(metrics.WidthPixels, metrics.HeightPixels);

            getMap().SetOnCameraIdleListener(mClusterManager);

            try
            {
                readItems();
            }
            catch (JSONException e)
            {
                Toast.MakeText(this, "Problem reading list of markers.", ToastLength.Long).Show();
            }
        }

        private void readItems()
        {
            Stream inputStream = Resources.OpenRawResource(Resource.Raw.radar_search);
            List<MyItem> items = new MyItemReader().read(inputStream);
            for (int i = 0; i< 100; i++)
            {
                double offset = i / 60d;
                foreach (MyItem item in items)
                {
                    LatLng position = item.Position;
                    double lat = position.Latitude + offset;
                    double lng = position.Longitude + offset;
                    MyItem offsetItem = new MyItem(lat, lng);
                    mClusterManager.AddItem(offsetItem);
                }
            }
        }
    }
}
