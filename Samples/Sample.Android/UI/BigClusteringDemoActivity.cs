using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;
using Android.Widget;
using Org.Json;
using Sample.Android.Models;
using Sample.Android.Utils;

namespace Sample.Android
{
    [Activity(Label = "BigClusteringDemoActivity")]
    public class BigClusteringDemoActivity : BaseDemoActivity
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
            for (int i = 0; i< 10; i++)
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
