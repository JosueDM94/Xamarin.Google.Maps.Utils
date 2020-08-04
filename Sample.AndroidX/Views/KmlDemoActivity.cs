using System;
using System.IO;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Data;
using Android.Gms.Maps.Utils.Data.Kml;
using Android.Util;
using Android.Widget;
using Java.IO;
using Java.Net;
using Org.XmlPull.V1;

namespace Sample.AndroidX
{
    [Activity(Label = "KmlDemoActivity")]
    public class KmlDemoActivity : BaseDemoActivity, KmlLayer.IOnFeatureClickListener
    {

        private GoogleMap mMap;
        private bool mIsRestore;

        protected override int getLayoutId()
        {
            return Resource.Layout.kml_demo;
        }

        protected override void startDemo(bool isRestore)
        {
            mIsRestore = isRestore;
            try
            {
                mMap = getMap();
                //retrieveFileFromResource();
                retrieveFileFromUrl();
            }
            catch (Exception e)
            {
                Log.Error("Exception caught", e.Message);
            }
        }

        private void retrieveFileFromResource()
        {
            LoadLocalKmlFile(Resource.Raw.campus);
        }

        private void retrieveFileFromUrl()
        {
            DownloadKmlFile(GetString(Resource.String.kml_url));
        }

        private void moveCameraToKml(KmlLayer kmlLayer)
        {
            if (mIsRestore) return;
            try
            {
                //Retrieve the first container in the KML layer
                KmlContainer container = (KmlContainer)kmlLayer.Containers.Iterator().Next();
                //Retrieve a nested container within the first container
                container = (KmlContainer)container.Containers.Iterator().Next();
                //Retrieve the first placemark in the nested container
                KmlPlacemark placemark = (KmlPlacemark)container.Placemarks.Iterator().Next();
                //Retrieve a polygon object in a placemark
                KmlPolygon polygon = (KmlPolygon)placemark.Geometry;
                //Create LatLngBounds of the outer coordinates of the polygon
                LatLngBounds.Builder builder = new LatLngBounds.Builder();
                foreach (LatLng latLng in polygon.OuterBoundaryCoordinates)
                {
                    builder.Include(latLng);
                }

                int width = Resources.DisplayMetrics.WidthPixels;
                int height = Resources.DisplayMetrics.HeightPixels;
                getMap().MoveCamera(CameraUpdateFactory.NewLatLngBounds(builder.Build(), width, height, 1));
            }
            catch (Java.Lang.Exception e)
            {
                // may fail depending on the KML being shown
                e.PrintStackTrace();
            }
        }

        //private Renderer.ImagesCache getImagesCache()
        //{
        //    RetainFragment retainFragment = RetainFragment.findOrCreateRetainFragment(SupportFragmentManager);
        //    return retainFragment.mImagesCache;
        //}

        /**
         * Fragment for retaining the bitmap cache between configuration changes.
         */
        //public static class RetainFragment : Fragment
        //{
        //    private static string TAG = nameof(RetainFragment));
        //    Renderer.ImagesCache mImagesCache;

        //    static RetainFragment findOrCreateRetainFragment(FragmentManager fm)
        //    {
        //        RetainFragment fragment = (RetainFragment)fm.FindFragmentByTag(TAG);
        //        if (fragment == null)
        //        {
        //            fragment = new RetainFragment();
        //            fm.BeginTransaction().Add(fragment, TAG).commit();
        //        }
        //        return fragment;
        //    }

        //    protected override void OnCreate(Bundle savedInstanceState)
        //    {
        //        base.OnCreate(savedInstanceState);
        //        setRetainInstance(true);
        //    }
        //}

        private void LoadLocalKmlFile(int mResourceId)
        {
            try
            {
                KmlLayer kmlLayer = new KmlLayer(mMap, mResourceId, this);
                addKmlToMap(kmlLayer);
            }
            catch (XmlPullParserException e)
            {
                e.PrintStackTrace();
            }
            catch (Java.IO.IOException e)
            {
                e.PrintStackTrace();
            }            
        }

        private void DownloadKmlFile(string mUrl)
        {
            try
            {
                Stream @is = new URL(mUrl).OpenStream();
                ByteArrayOutputStream buffer = new ByteArrayOutputStream();
                int nRead;
                byte[] data = new byte[16384];
                while ((nRead = @is.Read(data, 0, data.Length)) != -1)
                {
                    buffer.Write(data, 0, nRead);
                }
                buffer.Flush();
                try
                {
                    using (var stream = new MemoryStream(buffer.ToByteArray()))
                    {
                        KmlLayer kmlLayer = new KmlLayer(mMap, stream, this);//,
                                                                       //new MarkerManager(mMap),
                                                                       //new PolygonManager(mMap),
                                                                       //new PolylineManager(mMap),
                                                                       //new GroundOverlayManager(mMap),
                                                                       //getImagesCache());
                        addKmlToMap(kmlLayer);
                    }
                }
                catch (XmlPullParserException e)
                {
                    e.PrintStackTrace();
                }
            }
            catch (Java.IO.IOException e)
            {
                e.PrintStackTrace();
            }
        }

        private void addKmlToMap(KmlLayer kmlLayer)
        {
            if (kmlLayer != null)
            {
                kmlLayer.AddLayerToMap();
                kmlLayer.SetOnFeatureClickListener(this);
                moveCameraToKml(kmlLayer);
            }
        }

        public void OnFeatureClick(Feature p0)
        {
            Toast.MakeText(this,"Feature clicked: " + p0.Id,ToastLength.Short).Show();
        }
    }
}
