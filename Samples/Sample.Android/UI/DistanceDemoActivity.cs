using System.Collections.Generic;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils;
using Android.Widget;
using Java.Util;

namespace Sample.Android
{
    [Activity(Label = "DistanceDemoActivity")]
    public class DistanceDemoActivity : BaseDemoActivity, GoogleMap.IOnMarkerDragListener
    {
        private TextView mTextView;
        private Marker mMarkerA;
        private Marker mMarkerB;
        private Polyline mPolyline;

        protected override int getLayoutId()
        {
            return Resource.Layout.distance_demo;
        }

        protected override void startDemo(bool isRestore)
        {
            mTextView = FindViewById<TextView>(Resource.Id.textView);

            if (!isRestore)
            {
                getMap().MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(-33.8256, 151.2395), 10));
            }
            getMap().SetOnMarkerDragListener(this);

            mMarkerA = getMap().AddMarker(new MarkerOptions().SetPosition(new LatLng(-33.9046, 151.155)).Draggable(true));
            mMarkerB = getMap().AddMarker(new MarkerOptions().SetPosition(new LatLng(-33.8291, 151.248)).Draggable(true));
            mPolyline = getMap().AddPolyline(new PolylineOptions().Geodesic(true));

            Toast.MakeText(this, "Drag the markers!", ToastLength.Long).Show();
            showDistance();
        }

        private void showDistance()
        {
            double distance = SphericalUtil.ComputeDistanceBetween(mMarkerA.Position, mMarkerB.Position);
            mTextView.Text = "The markers are " + formatNumber(distance) + " apart.";
        }

        private void updatePolyline()
        {
            mPolyline.Points = (IList<LatLng>)Arrays.AsList(mMarkerA.Position, mMarkerB.Position);
        }

        private string formatNumber(double distance)
        {
            string unit = "m";
            if (distance < 1)
            {
                distance *= 1000;
                unit = "mm";
            }
            else if (distance > 1000)
            {
                distance /= 1000;
                unit = "km";
            }

            return string.Format("{0:####.000} {1}s", distance, unit);
        }

        public void OnMarkerDragEnd(Marker marker)
        {
            showDistance();
            updatePolyline();
        }

        
        public void OnMarkerDragStart(Marker marker)
        {

        }

        public void OnMarkerDrag(Marker marker)
        {
            showDistance();
            updatePolyline();
        }
    }
}
