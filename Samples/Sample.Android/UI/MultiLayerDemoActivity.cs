using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils;
using Android.Gms.Maps.Utils.Clustering;
using Android.Gms.Maps.Utils.Collections;
using Android.Gms.Maps.Utils.Data;
using Android.Gms.Maps.Utils.Data.GeoJson;
using Android.Gms.Maps.Utils.Data.Kml;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using Org.Json;
using Org.XmlPull.V1;
using Sample.Android.Models;
using Sample.Android.Utils;

namespace Sample.Android
{
	/**
     * Activity that adds multiple layers on the same map. This helps ensure that layers don't
     * interfere with one another.
     */
	[Activity(Label = "MultiLayerDemoActivity")]
	public class MultiLayerDemoActivity : BaseDemoActivity, GoogleMap.IOnMarkerClickListener
	{
		public static string TAG = "MultiDemo";

		private ClusterManager mClusterManager;

		protected override void startDemo(bool isRestore)
		{
			if (!isRestore)
			{
				getMap().MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(51.403186, -0.126446), 10));
			}

			// Shared object managers - used to support multiple layer types on the map simultaneously			
			MarkerManager markerManager = new MarkerManager(getMap());
			GroundOverlayManager groundOverlayManager = new GroundOverlayManager(getMap());
			PolygonManager polygonManager = new PolygonManager(getMap());
			PolylineManager polylineManager = new PolylineManager(getMap());

			// Add clustering			
			mClusterManager = new ClusterManager(this, getMap(), markerManager);
			getMap().SetOnCameraIdleListener(mClusterManager);

			try
			{
				readClusterItems();
			}
			catch (JSONException e)
			{
				Toast.MakeText(this, "Problem reading list of markers.", ToastLength.Long).Show();
				e.PrintStackTrace();
			}

			// Add GeoJSON from resource
			try
			{
				var geoJsonOnFeatureClickListener = new GeoJsonOnFeatureClickListener(this);

				// GeoJSON polyline
				GeoJsonLayer geoJsonLineLayer = new GeoJsonLayer(getMap(), Resource.Raw.south_london_line_geojson, this, markerManager, polygonManager, polylineManager, groundOverlayManager);
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
				GeoJsonLayer geoJsonPolygonLayer = new GeoJsonLayer(getMap(), Resource.Raw.south_london_square_geojson, this, markerManager, polygonManager, polylineManager, groundOverlayManager);
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
			catch (Java.IO.IOException)
			{
				Log.Error(TAG, "GeoJSON file could not be read");
			}
			catch (JSONException)
			{
				Log.Error(TAG, "GeoJSON file could not be converted to a JSONObject");
			}

			// Add KMLs from resources
			KmlLayer kmlPolylineLayer;
			KmlLayer kmlPolygonLayer;
			try
			{
				var kmlLayerOnFeatureClickListener = new KmlLayerOnFeatureClickListener(this);

				// KML Polyline				
				kmlPolylineLayer = new KmlLayer(getMap(), Resource.Raw.south_london_line_kml, this, markerManager, polygonManager, polylineManager, groundOverlayManager, null);
				kmlPolylineLayer.AddLayerToMap();
				kmlPolylineLayer.SetOnFeatureClickListener(kmlLayerOnFeatureClickListener);

				// KML Polygon
				kmlPolygonLayer = new KmlLayer(getMap(), Resource.Raw.south_london_square_kml, this, markerManager, polygonManager, polylineManager, groundOverlayManager, null);
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
			MarkerManager.Collection markerCollection = markerManager.NewCollection();
			markerCollection.AddMarker(new MarkerOptions()
					.SetPosition(new LatLng(51.150000, -0.150032))
					.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure))
					.SetTitle("Unclustered marker"));
			markerCollection.SetOnMarkerClickListener(this);
		}

		private void readClusterItems()
		{
			Stream inputStream = Resources.OpenRawResource(Resource.Raw.radar_search);
			List<MyItem> items = new MyItemReader().read(inputStream);
			mClusterManager.AddItems(items);
		}

		public bool OnMarkerClick(Marker marker)
		{
			Toast.MakeText(this, "Marker clicked: " + marker.Title, ToastLength.Short).Show();
			return false;
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