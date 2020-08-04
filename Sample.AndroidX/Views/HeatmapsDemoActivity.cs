
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.HeatMaps;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using Java.Util;
using Org.Json;

namespace Sample.AndroidX
{
    [Activity(Label = "HeatmapsDemoActivity")]
    public class HeatmapsDemoActivity : BaseDemoActivity
    {
        /**
         * Alternative radius for convolution
         */
        private static int ALT_HEATMAP_RADIUS = 10;

        /**
         * Alternative opacity of heatmap overlay
         */
        private static double ALT_HEATMAP_OPACITY = 0.4;

        /**
         * Alternative heatmap gradient (blue -> red)
         * Copied from Javascript version
         */
        private static int[] ALT_HEATMAP_GRADIENT_COLORS =
        {
            Color.Argb(0, 0, 255, 255),// transparent
            Color.Argb(255 / 3 * 2, 0, 255, 255),
            Color.Rgb(0, 191, 255),
            Color.Rgb(0, 0, 127),
            Color.Rgb(255, 0, 0)
        };

        public static float[] ALT_HEATMAP_GRADIENT_START_POINTS =
        {
            0.0f, 0.10f, 0.20f, 0.60f, 1.0f
        };

        public static Gradient ALT_HEATMAP_GRADIENT = new Gradient(ALT_HEATMAP_GRADIENT_COLORS,
                ALT_HEATMAP_GRADIENT_START_POINTS);

        private HeatmapTileProvider mProvider;
        private TileOverlay mOverlay;

        private bool mDefaultGradient = true;
        private bool mDefaultRadius = true;
        private bool mDefaultOpacity = true;

        /**
         * Maps name of data set to data (list of LatLngs)
         * Also maps to the URL of the data set for attribution
         */
        private Dictionary<string, DataSet> mLists = new Dictionary<string, DataSet>();

        protected override int getLayoutId()
        {
            return Resource.Layout.heatmaps_demo;
        }
        
        protected override void startDemo(bool isRestore)
        {
            if (!isRestore)
            {
                getMap().MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(-25, 143), 4));
            }

            // Set up the spinner/dropdown list
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);
            ArrayAdapter adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.heatmaps_datasets_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.OnItemSelectedListener = new SpinnerActivity(this);

            try
            {
                mLists.Add(GetString(Resource.String.police_stations), new DataSet(readItems(Resource.Raw.police), GetString(Resource.String.police_stations_url)));
                mLists.Add(GetString(Resource.String.medicare), new DataSet(readItems(Resource.Raw.medicare), GetString(Resource.String.medicare_url)));
            }
            catch (JSONException e)
            {
                Toast.MakeText(this, "Problem reading list of markers.", ToastLength.Long).Show();
            }

            // Make the handler deal with the map
            // Input: list of WeightedLatLngs, minimum and maximum zoom levels to calculate custom
            // intensity from, and the map to draw the heatmap on
            // radius, gradient and opacity not specified, so default are used
        }

        public void changeRadius(View view)
        {
            if (mDefaultRadius)
            {
                mProvider.SetRadius(ALT_HEATMAP_RADIUS);
            }
            else
            {
                mProvider.SetRadius(HeatmapTileProvider.DefaultRadius);
            }
            mOverlay.ClearTileCache();
            mDefaultRadius = !mDefaultRadius;
        }

        public void changeGradient(View view)
        {
            if (mDefaultGradient)
            {
                mProvider.SetGradient(ALT_HEATMAP_GRADIENT);
            }
            else
            {
                mProvider.SetGradient(HeatmapTileProvider.DefaultGradient);
            }
            mOverlay.ClearTileCache();
            mDefaultGradient = !mDefaultGradient;
        }

        public void changeOpacity(View view)
        {
            if (mDefaultOpacity)
            {
                mProvider.SetOpacity(ALT_HEATMAP_OPACITY);
            }
            else
            {
                mProvider.SetOpacity(HeatmapTileProvider.DefaultOpacity);
            }
            mOverlay.ClearTileCache();
            mDefaultOpacity = !mDefaultOpacity;
        }

        // Dealing with spinner choices
        public class SpinnerActivity : Java.Lang.Object, AdapterView.IOnItemSelectedListener
        {
            private HeatmapsDemoActivity activity;

            public SpinnerActivity(HeatmapsDemoActivity activity)
            {
                this.activity = activity;
            }

            public void OnItemSelected(AdapterView parent, View view, int pos, long id)
            {
                string dataset = parent.GetItemAtPosition(pos).ToString();

                TextView attribution = view.FindViewById<TextView>(Resource.Id.attribution);

                // Check if need to instantiate (avoid setData etc twice)
                if (activity.mProvider == null)
                {
                    activity.mProvider = new HeatmapTileProvider.Builder().Data(activity.mLists.GetValueOrDefault(activity.GetString(Resource.String.police_stations)).getData()).Build();
                    activity.mOverlay = activity.getMap().AddTileOverlay(new TileOverlayOptions().InvokeTileProvider(activity.mProvider));
                    // Render links
                    attribution.MovementMethod = LinkMovementMethod.Instance;
                }
                else
                {
                    activity.mProvider.SetData(activity.mLists.GetValueOrDefault(dataset).getData());
                    activity.mOverlay.ClearTileCache();
                }
                // Update attribution
                attribution.TextFormatted = Html.FromHtml(string.Format(activity.GetString(Resource.String.attrib_format), activity.mLists.GetValueOrDefault(dataset).getUrl()));

            }

            public void OnNothingSelected(AdapterView parent)
            {
                // Another interface callback
            }

        }
        // Datasets from http://data.gov.au
        private List<LatLng> readItems(int resource)
        {
            List<LatLng> list = new List<LatLng>();
            Stream inputStream = Resources.OpenRawResource(resource);
            string json = new Scanner(inputStream).UseDelimiter("\\A").Next();
            JSONArray array = new JSONArray(json);
            for (int i = 0; i < array.Length(); i++)
            {
                JSONObject jsonObject = array.GetJSONObject(i);
                double lat = jsonObject.GetDouble("lat");
                double lng = jsonObject.GetDouble("lng");
                list.Add(new LatLng(lat, lng));
            }
            return list;
        }        

        /**
         * Helper class - stores data sets and sources.
         */
        private class DataSet
        {
            public List<LatLng> mDataset { get; set; }
            private string mUrl;

            public DataSet(List<LatLng> dataSet, string url)
            {
                this.mDataset = dataSet;
                this.mUrl = url;
            }

            public List<LatLng> getData()
            {
                return mDataset;
            }

            public String getUrl()
            {
                return mUrl;
            }
        }
    }
}
