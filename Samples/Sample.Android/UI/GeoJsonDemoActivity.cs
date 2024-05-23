using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Data;
using Android.Gms.Maps.Utils.Data.GeoJson;
using Android.Util;
using Android.Widget;
using Java.IO;
using Org.Json;

namespace Sample.Android
{
    [Activity(Label = "GeoJsonDemoActivity")]
    public class GeoJsonDemoActivity : BaseDemoActivity, GeoJsonLayer.IGeoJsonOnFeatureClickListener
    {
        private static String mLogTag = "GeoJsonDemo";

        /**
         * Assigns a color based on the given magnitude
         */
        private static float magnitudeToColor(double magnitude)
        {
            if (magnitude < 1.0)
            {
                return BitmapDescriptorFactory.HueCyan;
            }
            else if (magnitude < 2.5)
            {
                return BitmapDescriptorFactory.HueGreen;
            }
            else if (magnitude < 4.5)
            {
                return BitmapDescriptorFactory.HueYellow;
            }
            else
            {
                return BitmapDescriptorFactory.HueRed;
            }
        }

        protected override int getLayoutId()
        {
            return Resource.Layout.geojson_demo;
        }


        protected override void startDemo(bool isRestore)
        {
            if (!isRestore)
            {
                getMap().MoveCamera(CameraUpdateFactory.NewLatLng(new LatLng(31.4118, -103.5355)));
            }
            // Download the GeoJSON file.
            //retrieveFileFromUrl();
            // Alternate approach of loading a local GeoJSON file.
            retrieveFileFromResource();
        }

        private void retrieveFileFromUrl()
        {
            DownloadGeoJsonFile(GetString(Resource.String.geojson_url));
        }

        private void retrieveFileFromResource()
        {
            try
            {
                GeoJsonLayer layer = new GeoJsonLayer(getMap(), Resource.Raw.earthquakes_with_usa, this);
                addGeoJsonLayerToMap(layer);
            }
            catch (Java.IO.IOException)
            {
                Log.Error(mLogTag, "GeoJSON file could not be read");
            }
            catch (JSONException)
            {
                Log.Error(mLogTag, "GeoJSON file could not be converted to a JSONObject");
            }
        }

        /**
         * Adds a point style to all features to change the color of the marker based on its magnitude
         * property
         */
        private void addColorsToMarkers(GeoJsonLayer layer)
        {
            // Iterate over all the features stored in the layer
            foreach (GeoJsonFeature feature in layer.Features.ToEnumerable())
            {
                // Check if the magnitude property exists
                if (feature.GetProperty("mag") != null && feature.HasProperty("place"))
                {
                    double magnitude = double.Parse(feature.GetProperty("mag"));

                    // Get the icon for the feature
                    BitmapDescriptor pointIcon = BitmapDescriptorFactory.DefaultMarker(magnitudeToColor(magnitude));

                    // Create a new point style
                    GeoJsonPointStyle pointStyle = new GeoJsonPointStyle();

                    // Set options for the point style
                    pointStyle.Icon = pointIcon;
                    pointStyle.Title = "Magnitude of " + magnitude;
                    pointStyle.Snippet = "Earthquake occured " + feature.GetProperty("place");

                    // Assign the point style to the feature
                    feature.PointStyle = pointStyle;
                }
            }
        }

        private async void DownloadGeoJsonFile(string url)
        {
            await Task.Factory.StartNew(() =>
            {
                using (var Client = new HttpClient())
                {
                    var response = Client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var layer = new GeoJsonLayer(getMap(), new JSONObject(response.Content.ReadAsStringAsync().Result));
                        this.RunOnUiThread(() =>
                        {
                            addGeoJsonLayerToMap(layer);
                        });
                    }
                }
            }).ConfigureAwait(false);
        }

        private void addGeoJsonLayerToMap(GeoJsonLayer layer)
        {

            addColorsToMarkers(layer);
            layer.AddLayerToMap();
            // Demonstrate receiving features via GeoJsonLayer clicks.
            layer.SetOnFeatureClickListener(this);
        }

        public void OnFeatureClick(Feature feature)
        {
            Toast.MakeText(this, "Feature clicked: " + feature.GetProperty("title"), ToastLength.Long).Show();
        }
    }
}