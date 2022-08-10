using System;
using System.Collections.Generic;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Google.Maps;
using Google.Maps.Utils;
using Newtonsoft.Json;
using Sample.iOS.Models;
using UIKit;

namespace Sample.iOS
{
    public class HeatmapViewController : UIViewController
    {
        private MapView mapView;
        private GMUHeatmapTileLayer heatmapLayer;
        private UIButton button;

        private UIColor[] gradientColors = new UIColor[] { UIColor.Green, UIColor.Red };
        private NSNumber[] gradientStartPoints = new NSNumber[] { 0.2, 1.0 };

        public override void LoadView()
        {
            var camera = CameraPosition.FromCamera(latitude: -37.848, longitude: 145.001, 10);
            mapView = MapView.FromCamera(frame: CGRect.Empty, camera: camera);
            mapView.Delegate = new MapDelegate();
            this.View = mapView;
            makeButton();
        }

        public override void ViewDidLoad()
        {
            // Set heatmap options.
            heatmapLayer = new GMUHeatmapTileLayer();
            heatmapLayer.Radius = 80;
            heatmapLayer.Opacity = 0.8f;
            heatmapLayer.Gradient = new GMUGradient(colors: gradientColors,
                                                startPoints: gradientStartPoints,
                                                mapSize: 256);
            addHeatmap();

            // Set the heatmap to the mapview.
            heatmapLayer.Map = mapView;
        }

        // Parse JSON data and add it to the heatmap layer.
        private void addHeatmap()
        {
            // Get the data: latitude/longitude positions of police stations.
            var items = new List<GMUWeightedLatLng>();
            try
            {
                if (NSBundle.MainBundle.GetUrlForResource("police_stations", "json") is NSUrl path)
                {
                    var data = NSData.FromUrl(path);
                    var json = data.ToString(NSStringEncoding.UTF8);
                    var objects = JsonConvert.DeserializeObject<List<Position>>(json);
                    if ( objects != null)
                    {
                        foreach (var item in objects)
                        {
                            var lat = item.Latitude;
                            var lng = item.Longitude;
                            var coords = new GMUWeightedLatLng(coordinate: new CLLocationCoordinate2D(latitude: item.Latitude, longitude: item.Longitude), intensity: 1.0f);
                            items.Add(coords);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Could not read the JSON.");
                    }
                }
            }
            catch(Exception error)
            {
                Console.WriteLine(error.Message);
            }
            // Add the latlngs to the heatmap layer.
            heatmapLayer.WeightedData = items.ToArray();
        }

        [Export("removeHeatmap")]
        private void removeHeatmap()
        {
            heatmapLayer.Map = null;
            heatmapLayer = null;
            // Disable the button to prevent subsequent calls, since heatmapLayer is now nil.
            button.Enabled = false;
        }

        private void makeButton()
        {
            // A button to test removing the heatmap.
            button = new UIButton(frame: new CGRect(x: 5, y: 150, width: 200, height: 35));
            button.BackgroundColor = UIColor.Blue;
            button.Alpha = 0.5f;
            button.SetTitle("Remove heatmap", forState: UIControlState.Normal);
            button.AddTarget(this, sel: new ObjCRuntime.Selector("removeHeatmap"), UIControlEvent.TouchUpInside);
            this.mapView.AddSubview(button);
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
