using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Org.Json;
using Android.App;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Data;
using Android.Gms.Maps.Utils.Data.GeoJson;

using Sample.Droid.Views.Base;

namespace Sample.Droid.Views.GeoJson
{
    [Activity(Label = "GeoJsonActivity")]
    public class GeoJsonActivity : BaseActivity, GeoJsonLayer.IGeoJsonOnFeatureClickListener
    {
        protected override void StartMap()
        {
            // Download the GeoJSON file.
            RetrieveFileFromUrl();
            // Alternate approach of loading a local GeoJSON file.
            //RetrieveFileFromResource();
        }

        protected override int LayoutId()
        {
            return Resource.Layout.GeoJsonLayout;
        }

        private static float MagnitudeToColor(double magnitude)
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

        private void RetrieveFileFromUrl()
        {
            DownloadGeoJsonFile(GetString(Resource.String.geojson_url));
        }

        private void RetrieveFileFromResource()
        {
            try
            {
                GeoJsonLayer layer = new GeoJsonLayer(googleMap, Resource.Raw.earthquakes_with_usa, this);
                AddGeoJsonLayerToMap(layer);
            }
            catch (Exception)
            {
                Console.WriteLine("GeoJSON file could not be read");
            }
        }

        private void AddGeoJsonLayerToMap(GeoJsonLayer layer)
        {
            AddColorsToMarkers(layer);
            layer.AddLayerToMap();
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLng(new LatLng(31.4118, -103.5355)));
            // Demonstrate receiving features via GeoJsonLayer clicks.
            layer.SetOnFeatureClickListener(this);
        }

        private void AddColorsToMarkers(GeoJsonLayer layer)
        {
            // Iterate over all the features stored in the layer
            foreach (GeoJsonFeature feature in layer.Features.ToEnumerable())
            {
                // Check if the magnitude property exists
                if (feature.GetProperty("mag") != null && feature.HasProperty("place"))
                {
                    double magnitude = Double.Parse(feature.GetProperty("mag"));

                    // Get the icon for the feature
                    BitmapDescriptor pointIcon = BitmapDescriptorFactory.DefaultMarker(MagnitudeToColor(magnitude));

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
                        var layer = new GeoJsonLayer(googleMap, new JSONObject(response.Content.ReadAsStringAsync().Result));
                        this.RunOnUiThread(() =>
                        {
                            AddGeoJsonLayerToMap(layer);
                        });
                    }
                }
            }).ConfigureAwait(false);
        }

        public void OnFeatureClick(Feature p0)
        {
            Toast.MakeText(this, "Feature clicked: " + p0.GetProperty("title"), ToastLength.Short).Show();
        }
    }
}