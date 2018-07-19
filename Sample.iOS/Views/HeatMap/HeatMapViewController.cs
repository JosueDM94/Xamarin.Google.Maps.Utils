using System;
using Newtonsoft.Json;
using System.Collections.Generic;

using UIKit;
using Foundation;
using Google.Maps;
using CoreGraphics;
using CoreLocation;
using Google.Maps.Utils;

using Sample.iOS.Models;

namespace Sample.iOS.Views.HeatMap
{
    public partial class HeatMapViewController : UIViewController
    {
        private double cameraLatitude = -37.848;
        private double cameraLongitude = 145.001;

        private MapView mapView;
        private GMUHeatmapTileLayer heatmapTileLayer;

        private UIColor[] gradientColors = { UIColor.Green, UIColor.Red };
        private NSNumber[] gradientStartPoints = { 0.2, 1.0 };

        public HeatMapViewController() : base("HeatMapViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetHeatMap();
            SetRemoveButton();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void LoadView()
        {
            base.LoadView();
            SetMapView();
        }

        private void AddHeatMap()
        {
            var items = new List<GMUWeightedLatLng>();
            var bundle = NSBundle.MainBundle;
            var path = bundle.GetUrlForResource("police_stations", "json");
            var data = NSData.FromUrl(path);
            var json = data.ToString(NSStringEncoding.UTF8);
            var JsonItems = JsonConvert.DeserializeObject<List<Position>>(json);
            foreach(var item in JsonItems)
                items.Add(new GMUWeightedLatLng(new CLLocationCoordinate2D(item.Latitude, item.Longitude), 1.0f));
            heatmapTileLayer.WeightedData = items.ToArray();
        }

        private void SetMapView()
        {
            var mapDelegate = new MapDelegate();
            var camera = CameraPosition.FromCamera(cameraLatitude, cameraLongitude, 4);
            mapView = MapView.FromCamera(CGRect.Empty, camera);
            mapView.Delegate = mapDelegate;
            View = mapView;
        }

        private void SetHeatMap()
        {
            heatmapTileLayer = new GMUHeatmapTileLayer();
            heatmapTileLayer.Radius = 80;
            heatmapTileLayer.Opacity = 0.8f;
            heatmapTileLayer.Gradient = new GMUGradient(gradientColors, gradientStartPoints, 256);
            AddHeatMap();
            heatmapTileLayer.Map = mapView;
        }

        private void SetRemoveButton()
        {
            UIBarButtonItem removeButton = new UIBarButtonItem()
            {
                Target = this,
                Title = "Remove",
                Style = UIBarButtonItemStyle.Plain
            };
            removeButton.Clicked -= RemoveButton_Clicked;
            removeButton.Clicked += RemoveButton_Clicked;
            NavigationItem.RightBarButtonItems = new UIBarButtonItem[] { removeButton };
        }

        void RemoveButton_Clicked(object sender, EventArgs e)
        {
            if(heatmapTileLayer!=null)
            {
                heatmapTileLayer.Map = null;
                heatmapTileLayer = null;
            }
        }

        private class MapDelegate : MapViewDelegate
        {
            public override void DidTapAtCoordinate(MapView mapView, CLLocationCoordinate2D coordinate)
            {
                Console.WriteLine("You tapped at " + coordinate.Latitude + ", " + coordinate.Longitude);
            }
        }
    }
}

