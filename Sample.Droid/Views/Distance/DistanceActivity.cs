using System;
using System.Collections.Generic;

using Android.App;
using Android.Widget;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils;

using Sample.Droid.Views.Base;

namespace Sample.Droid.Views.Distance
{
    [Activity(Label = "DistanceActivity")]
    public class DistanceActivity : BaseActivity,GoogleMap.IOnMarkerDragListener
    {
        private TextView textView;
        private Marker markerA;
        private Marker markerB;
        private Polyline polyline;

        protected override void StartMap()
        {
            textView = FindViewById<TextView>(Resource.Id.textView);
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(-33.8256, 151.2395), 10));
            googleMap.SetOnMarkerDragListener(this);

            markerA = googleMap.AddMarker(new MarkerOptions().SetPosition(new LatLng(-33.9046, 151.155)).Draggable(true));
            markerB = googleMap.AddMarker(new MarkerOptions().SetPosition(new LatLng(-33.8291, 151.248)).Draggable(true));
            polyline = googleMap.AddPolyline(new PolylineOptions().Geodesic(true));

            Toast.MakeText(this, "Drag the markers!", ToastLength.Long).Show();
            ShowDistance();
        }

        protected override int LayoutId()
        {
            return Resource.Layout.DistanceLayout;
        }

        private void ShowDistance()
        {
            double distance = SphericalUtil.ComputeDistanceBetween(markerA.Position, markerB.Position);
            textView.Text = "The markers are " + FormatNumber(distance) + " apart.";
        }

        private String FormatNumber(double distance)
        {
            String unit = "m";
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

            return String.Format("{0:####.000} {1}s", distance, unit);
        }

        private void UpdatePolyline()
        {
            polyline.Points = new List<LatLng> { markerA.Position,markerB.Position };
        }

        public void OnMarkerDrag(Marker marker)
        {
            ShowDistance();
            UpdatePolyline();
        }

        public void OnMarkerDragEnd(Marker marker)
        {
            ShowDistance();
            UpdatePolyline();
        }

        public void OnMarkerDragStart(Marker marker)
        {
            
        }
    }
}
