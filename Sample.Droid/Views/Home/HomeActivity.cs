using System;

using Android.OS;
using Android.App;
using Android.Widget;
using Android.Content;

using Sample.Droid.Views.Kml;
using Sample.Droid.Views.GeoJson;
using Sample.Droid.Views.HeatMap;
using Sample.Droid.Views.Distance;
using Sample.Droid.Views.Clustering;
using Sample.Droid.Views.PolyDecode;
using Sample.Droid.Views.PolySimplify;
using Sample.Droid.Views.IconGenerato;
using Sample.Droid.Views.HeatMapPlaces;
using Sample.Droid.Views.BigClustering;
using Sample.Droid.Views.TileProjection;
using Sample.Droid.Views.VisibleClustering;
using Sample.Droid.Views.CustomMarkerClustering;

namespace Sample.Droid.Views.Home
{
    [Activity(Label = "Xamarin Maps Utils", MainLauncher = true)]
    public class HomeActivity : Activity
    {
        private Button btnClustering,btnBigClustering,btnCustomClustering,btnDistance,btnGeoJson;
        private Button btnHeatMap, btnHeatMapPlaces, btnIconGenerator, btnKml, btnPolyDecode;
        private Button btnPolySimplify, btnTileProjection, btnVisibleClustering;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.HomeLayout);

            InitElements();
        }

        protected override void OnStart()
        {
            base.OnStart();
            AddEventHandlers();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnStop()
        {
            base.OnStop();
            RemoveEventHandlers();
        }

        private void InitElements()
        {
            btnKml = FindViewById<Button>(Resource.Id.btnKml);
            btnHeatMap = FindViewById<Button>(Resource.Id.btnHeatMap);
            btnGeoJson = FindViewById<Button>(Resource.Id.btnGeoJson);
            btnDistance = FindViewById<Button>(Resource.Id.btnDistance);
            btnPolyDecode = FindViewById<Button>(Resource.Id.btnPolyDecode);
            btnClustering = FindViewById<Button>(Resource.Id.btnClustering);
            btnPolySimplify = FindViewById<Button>(Resource.Id.btnPolySimplify);
            btnHeatMapPlaces = FindViewById<Button>(Resource.Id.btnHeatMapPlaces);
            btnIconGenerator = FindViewById<Button>(Resource.Id.btnIconGenerator);
            btnBigClustering = FindViewById<Button>(Resource.Id.btnBigClustering);
            btnTileProjection = FindViewById<Button>(Resource.Id.btnTileProjection);
            btnCustomClustering = FindViewById<Button>(Resource.Id.btnCustomClustering);
            btnVisibleClustering = FindViewById<Button>(Resource.Id.btnVisibleClustering);
        }

        private void AddEventHandlers()
        {
            HasMapKey();
            btnKml.Click += BtnKml_Click;
            btnHeatMap.Click += BtnHeatMap_Click;
            btnGeoJson.Click += BtnGeoJson_Click;
            btnDistance.Click += BtnDistance_Click;
            btnPolyDecode.Click += BtnPolyDecode_Click;
            btnClustering.Click += BtnClustering_Click;
            btnPolySimplify.Click += BtnPolySimplify_Click;
            btnHeatMapPlaces.Click += BtnHeatMapPlaces_Click;
            btnIconGenerator.Click += BtnIconGenerator_Click;
            btnBigClustering.Click += BtnBigClustering_Click;
            btnTileProjection.Click += BtnTileProjection_Click;
            btnCustomClustering.Click += BtnCustomClustering_Click;
            btnVisibleClustering.Click += BtnVisibleClustering_Click;
        }

        private void RemoveEventHandlers()
        {
            HasMapKey();
            btnKml.Click -= BtnKml_Click;
            btnHeatMap.Click -= BtnHeatMap_Click;
            btnGeoJson.Click -= BtnGeoJson_Click;
            btnDistance.Click -= BtnDistance_Click;
            btnPolyDecode.Click -= BtnPolyDecode_Click;
            btnClustering.Click -= BtnClustering_Click;
            btnPolySimplify.Click -= BtnPolySimplify_Click;
            btnHeatMapPlaces.Click -= BtnHeatMapPlaces_Click;
            btnIconGenerator.Click -= BtnIconGenerator_Click;
            btnBigClustering.Click -= BtnBigClustering_Click;
            btnTileProjection.Click -= BtnTileProjection_Click;
            btnCustomClustering.Click -= BtnCustomClustering_Click;
            btnVisibleClustering.Click -= BtnVisibleClustering_Click;
        }

        void BtnKml_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(KmlActivity)))
                StartActivity(intent);
        }

        void BtnHeatMap_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(HeatMapActivity)))
                StartActivity(intent);
        }

        void BtnPolyDecode_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(PolyDecodeActivity)))
                StartActivity(intent);
        }

        void BtnHeatMapPlaces_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(HeatMapPlacesActivity)))
                StartActivity(intent);
        }

        void BtnIconGenerator_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(IconGeneratorActivity)))
                StartActivity(intent);
        }

        void BtnPolySimplify_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(PolySimplifyActivity)))
                StartActivity(intent);
        }

        void BtnTileProjection_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(TileProjectionActivity)))
                StartActivity(intent);
        }

        void BtnGeoJson_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(GeoJsonActivity)))
                StartActivity(intent);
        }

        void BtnVisibleClustering_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(VisibleClusteringActivity)))
                StartActivity(intent);
        }

        void BtnDistance_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(DistanceActivity)))
                StartActivity(intent);
        }

        void BtnClustering_Click(object sender, EventArgs e)
        {
            using(var intent = new Intent(this,typeof(ClusteringActivity)))
                StartActivity(intent);
        }

        void BtnBigClustering_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(BigClusteringActivity)))
                StartActivity(intent);
        }

        void BtnCustomClustering_Click(object sender, EventArgs e)
        {
            using (var intent = new Intent(this, typeof(CustomMarkerClusteringActivity)))
                StartActivity(intent);
        }

        private void HasMapKey()
        {
            if (string.IsNullOrEmpty(GetString(Resource.String.maps_api_key)))
            {
                Toast.MakeText(this, "Add your own API key in demo/secure.properties as MAPS_API_KEY=YOUR_API_KEY", ToastLength.Long).Show();
            }
        }
    }
}
