using System;
using System.IO;
using System.Collections.Generic;

using Android.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Android.Gms.Maps;
using Android.Graphics;
using Android.Text.Method;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.HeatMaps;

using Sample.Droid.Utils;
using Sample.Droid.Views.Base;
using Android.OS;

namespace Sample.Droid.Views.HeatMap
{
    [Activity(Label = "HeatMapActivity")]
    public class HeatMapActivity : BaseActivity,AdapterView.IOnItemSelectedListener
    {
        private int ALT_HEATMAP_RADIUS = 10;
        private double ALT_HEATMAP_OPACITY = 0.4;

        private static int[] ALT_HEATMAP_GRADIENT_COLORS = {
            Color.Argb(0, 0, 255, 255),// transparent
            Color.Argb(255 / 3 * 2, 0, 255, 255),
            Color.Rgb(0, 191, 255),
            Color.Rgb(0, 0, 127),
            Color.Rgb(255, 0, 0)
        };

        public static float[] ALT_HEATMAP_GRADIENT_START_POINTS = {
            0.0f, 0.10f, 0.20f, 0.60f, 1.0f
        };

        public static Gradient ALT_HEATMAP_GRADIENT = new Gradient(ALT_HEATMAP_GRADIENT_COLORS, ALT_HEATMAP_GRADIENT_START_POINTS);

        private HeatmapTileProvider provider;
        private TileOverlay overlay;

        private bool defaultGradient = true;
        private bool defaultRadius = true;
        private bool defaultOpacity = true;

        private Dictionary<string,DataSet> list = new Dictionary<string, DataSet>();

        private Button btnRadius, btnOpacity, btnGradiant;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            InitElements();
        }

        protected override void StartMap()
        {
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(-25, 143), 4));
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem,Resources.GetStringArray(Resource.Array.heatmaps_datasets_array));
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.OnItemSelectedListener = this;
            try
            {
                list.Add(GetString(Resource.String.police_stations), new DataSet(ReadItems(Resource.Raw.police), GetString(Resource.String.police_stations_url)));
                list.Add(GetString(Resource.String.medicare), new DataSet(ReadItems(Resource.Raw.medicare), GetString(Resource.String.medicare_url)));
            }
            catch(Exception)
            {
                Toast.MakeText(this, "Problem reading list of markers.", ToastLength.Long).Show();
            }
        }

        protected override int LayoutId()
        {
            return Resource.Layout.HeatMapsLayout;
        }

        protected override void OnStart()
        {
            base.OnStart();
            btnRadius.Click += BtnRadius_Click;
            btnGradiant.Click += BtnGradiant_Click;
            btnOpacity.Click += BtnOpacity_Click;
        }

        protected override void OnStop()
        {
            base.OnStop();
            btnRadius.Click -= BtnRadius_Click;
            btnGradiant.Click -= BtnGradiant_Click;
            btnOpacity.Click -= BtnOpacity_Click;
        }

        public void OnItemSelected(AdapterView parent, View view, int position, long id)
        {
            DataSet dataset;
            string name = parent.GetItemAtPosition(position).ToString();
            TextView attribution = FindViewById<TextView>(Resource.Id.attribution);
            if(provider == null)
            {
                list.TryGetValue(GetString(Resource.String.police_stations), out dataset);
                provider = new HeatmapTileProvider.Builder().Data(dataset.dataSet).Build();
                overlay = googleMap.AddTileOverlay(new TileOverlayOptions().InvokeTileProvider(provider));
                attribution.MovementMethod = LinkMovementMethod.Instance;
            }
            else{
                list.TryGetValue(name, out dataset);
                provider.SetData(dataset.dataSet);
                overlay.ClearTileCache();
            }
            list.TryGetValue(name, out dataset);
            attribution.TextFormatted = Html.FromHtml(string.Format(GetString(Resource.String.attrib_format), dataset.url),FromHtmlOptions.ModeCompact);
        }

        private void InitElements()
        {
            btnRadius = FindViewById<Button>(Resource.Id.btnRadius);
            btnOpacity = FindViewById<Button>(Resource.Id.btnOpacity);
            btnGradiant = FindViewById<Button>(Resource.Id.btnGradiant);
        }

        public void OnNothingSelected(AdapterView parent)
        {
            
        }

        void BtnRadius_Click(object sender, EventArgs e)
        {
            if (defaultRadius)
            {
                provider.SetRadius(ALT_HEATMAP_RADIUS);
            }
            else
            {
                provider.SetRadius(HeatmapTileProvider.DefaultRadius);
            }
            overlay.ClearTileCache();
            defaultRadius = !defaultRadius;
        }

        void BtnGradiant_Click(object sender, EventArgs e)
        {
            if (defaultGradient)
            {
                provider.SetGradient(ALT_HEATMAP_GRADIENT);
            }
            else
            {
                provider.SetGradient(HeatmapTileProvider.DefaultGradient);
            }
            overlay.ClearTileCache();
            defaultGradient = !defaultGradient;
        }

        void BtnOpacity_Click(object sender, EventArgs e)
        {
            if (defaultOpacity)
            {
                provider.SetOpacity(ALT_HEATMAP_OPACITY);
            }
            else
            {
                provider.SetOpacity(HeatmapTileProvider.DefaultOpacity);
            }
            overlay.ClearTileCache();
            defaultOpacity = !defaultOpacity;
        }

        private List<LatLng> ReadItems(int resource)
        {            
            Stream inputStream = Resources.OpenRawResource(resource);
            return ItemReader.StreamToLatLng(inputStream);
        }

        private class DataSet
        {
            public DataSet(List<LatLng> dataSet,string url)
            {
                this.url = url;
                this.dataSet = dataSet;
            }

            public string url { get; set; }
            public List<LatLng> dataSet { get; set; }
        }
    }
}