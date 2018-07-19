﻿using System;
using System.IO;

using Android.App;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;

using Sample.Droid.Utils;
using Sample.Droid.Models;
using Sample.Droid.Views.Base;

namespace Sample.Droid.Views.BigClustering
{
    [Activity(Label = "BigClusteringActivity")]
    public class BigClusteringActivity : BaseActivity
    {
        private ClusterManager clusterManager;

        protected override void StartMap()
        {
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(51.503186, -0.126446), 10));
            clusterManager = new ClusterManager(this, googleMap);
            googleMap.SetOnCameraIdleListener(clusterManager);

            try
            {
                ReadJson();
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Problem reading list of markers.", ToastLength.Long).Show();
            }
        }

        private void ReadJson()
        {
            Stream stream = Resources.OpenRawResource(Resource.Raw.radar_search);
            var items = ItemReader.StreamToClusterMarker(stream);
            for (int i = 0; i < 10; i++)
            {
                double offset = i / 60d;
                foreach (var item in items)
                {
                    var position = item.Position;
                    double lat = position.Latitude + offset;
                    double lng = position.Longitude + offset;
                    var offsetItem = new ClusterMarker(lat, lng);
                    clusterManager.AddItem(offsetItem);
                }
            }
        }
    }
}