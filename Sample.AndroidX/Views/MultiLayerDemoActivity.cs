using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils;
using Android.Gms.Maps.Utils.Clustering;
using Android.Gms.Maps.Utils.Data;
using Android.Gms.Maps.Utils.Data.GeoJson;
using Android.Gms.Maps.Utils.Data.Kml;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using Org.Json;
using Org.XmlPull.V1;
using Sample.AndroidX.Models;
using Sample.AndroidX.Utils;

namespace Sample.AndroidX
{
    /**
     * Activity that adds multiple layers on the same map. This helps ensure that layers don't
     * interfere with one another.
     */
    [Activity(Label = "MultiLayerDemoActivity")]
    public class MultiLayerDemoActivity : BaseDemoActivity, GoogleMap.IOnMarkerClickListener
    {
        public static string TAG = "MultiDemo";

        public bool OnMarkerClick(Marker marker)
        {
            Toast.MakeText(this, "Marker clicked: " + marker.Title, ToastLength.Short).Show();
            return false;
        }

        protected override void startDemo(bool isRestore)
        {
            if (!isRestore)
            {
                getMap().MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(51.403186, -0.126446), 10));
            }

            // Shared object managers - used to support multiple layer types on the map simultaneously
            // [START maps_multilayer_demo_init1]
            MarkerManager markerManager = new MarkerManager(getMap());
            //GroundOverlayManager groundOverlayManager = new GroundOverlayManager(getMap());
            //PolygonManager polygonManager = new PolygonManager(getMap());
            //PolylineManager polylineManager = new PolylineManager(getMap());
            // [END maps_multilayer_demo_init1]

            // Add clustering
            // [START maps_multilayer_demo_init2]
            ClusterManager clusterManager = new ClusterManager(this, getMap(), markerManager);
            // [END maps_multilayer_demo_init2]
            getMap().SetOnCameraIdleListener(clusterManager);
            addClusterItems(clusterManager);

            // Add GeoJSON from resource
            try
            {
                var geoJsonOnFeatureClickListener = new GeoJsonOnFeatureClickListener(this);
                // GeoJSON polyline
                // [START maps_multilayer_demo_init3]
                GeoJsonLayer geoJsonLineLayer = new GeoJsonLayer(getMap(), Resource.Raw.south_london_line_geojson, this);//, markerManager, polygonManager, polylineManager, groundOverlayManager);
                // [END maps_multilayer_demo_init3]
                // Make the line red
                GeoJsonLineStringStyle geoJsonLineStringStyle = new GeoJsonLineStringStyle();
                geoJsonLineStringStyle.Color = Color.Red;
                foreach (GeoJsonFeature f in geoJsonLineLayer.Features.ToEnumerable())
                {
                    f.LineStringStyle = geoJsonLineStringStyle;
                }
                geoJsonLineLayer.AddLayerToMap();
                geoJsonLineLayer.SetOnFeatureClickListener(geoJsonOnFeatureClickListener);

                // GeoJSON polygon
                GeoJsonLayer geoJsonPolygonLayer = new GeoJsonLayer(getMap(), Resource.Raw.south_london_square_geojson, this);//, markerManager, polygonManager, polylineManager, groundOverlayManager);
                // Fill it with red
                GeoJsonPolygonStyle geoJsonPolygonStyle = new GeoJsonPolygonStyle();
                geoJsonPolygonStyle.FillColor = Color.Red;
                foreach (GeoJsonFeature f in geoJsonPolygonLayer.Features.ToEnumerable())
                {
                    f.PolygonStyle = geoJsonPolygonStyle;
                }
                geoJsonPolygonLayer.AddLayerToMap();
                geoJsonPolygonLayer.SetOnFeatureClickListener(geoJsonOnFeatureClickListener);
            }
            catch (Java.IO.IOException e)
            {
                Log.Error(TAG, "GeoJSON file could not be read");
            }
            catch (JSONException e)
            {
                Log.Error(TAG, "GeoJSON file could not be converted to a JSONObject");
            }

            // Add KMLs from resources
            try
            {
                var kmlLayerOnFeatureClickListener = new KmlLayerOnFeatureClickListener(this);
                // KML Polyline
                // [START maps_multilayer_demo_init4]
                KmlLayer kmlPolylineLayer = new KmlLayer(getMap(), Resource.Raw.south_london_line_kml, this);//, markerManager, polygonManager, polylineManager, groundOverlayManager, null);
                // [END maps_multilayer_demo_init4]
                // [START maps_multilayer_demo_init6]
                kmlPolylineLayer.AddLayerToMap();
                kmlPolylineLayer.SetOnFeatureClickListener(kmlLayerOnFeatureClickListener);
                // [END maps_multilayer_demo_init6]

                // KML Polygon
                KmlLayer kmlPolygonLayer = new KmlLayer(getMap(), Resource.Raw.south_london_square_kml, this);//, markerManager, polygonManager, polylineManager, groundOverlayManager, null);
                kmlPolygonLayer.AddLayerToMap();
                kmlPolygonLayer.SetOnFeatureClickListener(kmlLayerOnFeatureClickListener);
            }
            catch (XmlPullParserException e)
            {
                e.PrintStackTrace();
            }
            catch (Java.IO.IOException e)
            {
                e.PrintStackTrace();
            }

            // Unclustered marker - instead of adding to the map directly, use the MarkerManager
            // [START maps_multilayer_demo_init5]
            MarkerManager.Collection markerCollection = markerManager.NewCollection();
            markerCollection.AddMarker(new MarkerOptions()
                    .SetPosition(new LatLng(51.150000, -0.150032))
                    .SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure))
                    .SetTitle("Unclustered marker"));
            // [END maps_multilayer_demo_init5]
            // [START maps_multilayer_demo_init7]
            markerCollection.SetOnMarkerClickListener(this);
            // [END maps_multilayer_demo_init7]
        }

        private void addClusterItems(ClusterManager clusterManager)
        {
            Stream inputStream = Resources.OpenRawResource(Resource.Raw.radar_search);
            List<MyItem> items;
            try
            {
                items = new MyItemReader().read(inputStream);
                clusterManager.AddItems(items);
            }
            catch (JSONException e)
            {
                Toast.MakeText(this, "Problem reading list of markers.", ToastLength.Long).Show();
                e.PrintStackTrace();
            }
        }

        private class KmlLayerOnFeatureClickListener : Java.Lang.Object, KmlLayer.IOnFeatureClickListener
        {
            private Context context;

            public KmlLayerOnFeatureClickListener(Context context)
            {
                this.context = context;
            }

            public void OnFeatureClick(Feature p0)
            {
                Toast.MakeText(context, "KML polyline clicked: " + p0.GetProperty("name"), ToastLength.Long).Show();
            }
        }

        private class GeoJsonOnFeatureClickListener : Java.Lang.Object, GeoJsonLayer.IGeoJsonOnFeatureClickListener
        {
            private Context context;

            public GeoJsonOnFeatureClickListener(Context context)
            {
                this.context = context;
            }

            public void OnFeatureClick(Feature p0)
            {
                Toast.MakeText(context, "GeoJSON polygon clicked: " + p0.GetProperty("title"), ToastLength.Short).Show();
            }
        }
    }
}
