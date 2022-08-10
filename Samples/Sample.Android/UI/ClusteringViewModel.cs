using System.Collections.Generic;
using System.IO;
using Android.Content.Res;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering.Algorithm;
using AndroidX.Lifecycle;
using Sample.Android.Utils;
using Sample.Android.Models;

namespace Sample.Android
{
    public class ClusteringViewModel : ViewModel
    {
        private NonHierarchicalDistanceBasedAlgorithm mAlgorithm = new NonHierarchicalDistanceBasedAlgorithm();

        public NonHierarchicalDistanceBasedAlgorithm getAlgorithm()
        {
            return mAlgorithm;
        }

        public void readItems(Resources resources)
        {
            Stream inputStream = resources.OpenRawResource(Resource.Raw.radar_search);
            List<MyItem> items = new MyItemReader().read(inputStream);
            //mAlgorithm.lock();
            try
            {
                for (int i = 0; i< 100; i++)
                {
                    double offset = i / 60d;
                    foreach (MyItem item in items)
                    {
                        LatLng position = item.Position;
                        double lat = position.Latitude + offset;
                        double lng = position.Longitude + offset;
                        MyItem offsetItem = new MyItem(lat, lng);
                        mAlgorithm.AddItem(offsetItem);
                    }
                }
            }
            finally
            {
                //mAlgorithm.unlock();
            }
        }
    }
}
