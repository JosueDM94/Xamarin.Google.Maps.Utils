using System;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Google.Maps;
using Google.Maps.Utils;
using Sample.iOS.Models;
using UIKit;

namespace Sample.iOS
{
    public class BasicViewController : UIViewController, IGMUClusterManagerDelegate, IGMUClusterRendererDelegate
    {
        double kClusterItemCount = 10000;
        double kCameraLatitude = -33.8;
        double kCameraLongitude = 151.2;

        private MapView mapView;
        private GMUClusterManager clusterManager;

        public override void LoadView()
        {
            var camera = CameraPosition.FromCamera(latitude: kCameraLatitude, longitude: kCameraLongitude, zoom: 10);
            mapView = MapView.FromCamera(frame: CGRect.Empty, camera: camera);
            this.View = mapView;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Set up the cluster manager with default icon generator and renderer.
            var iconGenerator = new GMUDefaultClusterIconGenerator();
            var algorithm = new GMUNonHierarchicalDistanceBasedAlgorithm();
            var renderer = new GMUDefaultClusterRenderer(mapView: mapView, iconGenerator: iconGenerator);
            clusterManager = new GMUClusterManager(mapView: mapView, algorithm: algorithm, renderer: renderer);

            // Generate and add random items to the cluster manager.
            generateClusterItems();

            // Call cluster() after items have been added to perform the clustering and rendering on map.
            clusterManager.Cluster();

            // Register self to listen to both GMUClusterManagerDelegate and GMSMapViewDelegate events.
            var mapDelegate = new MapDelegate();
            clusterManager.SetDelegate(this, mapDelegate: mapDelegate);

            UIBarButtonItem removeButton = new UIBarButtonItem("Remove", UIBarButtonItemStyle.Plain, this, new ObjCRuntime.Selector("removeClusterManager"));
            NavigationItem.RightBarButtonItems = new UIBarButtonItem[] { removeButton };
        }

        [Export("clusterManager:didTapCluster:")]
        public bool DidTapCluster(GMUClusterManager clusterManager, IGMUCluster cluster)
        {
            var newCamera = CameraPosition.FromCamera(cluster.Position, zoom: mapView.Camera.Zoom + 1);
            var update = CameraUpdate.SetCamera(newCamera);
            mapView.MoveCamera(update);
            return false;
        }

        // Randomly generates cluster items within some extent of the camera and adds them to the cluster manager.
        private void generateClusterItems()
        {
            var extent = 0.2;
            for (int index = 1; index <= kClusterItemCount; index++)
            {
                var lat = kCameraLatitude + extent * randomScale();
                var lng = kCameraLongitude + extent * randomScale();
                var name = $"Item {index}";
                var item = new POIItem(position: new CLLocationCoordinate2D(lat, lng), name: name);
                clusterManager.AddItem(item);
            }
        }

        // Returns a random value between -1.0 and 1.0.
        private double randomScale()
        {
            Random random = new Random();
            return random.NextDouble() * (1.0 - -1.0) + -1.0;
        }

        private GMUDefaultClusterIconGenerator defaultIconGenerator()
        {
            return new GMUDefaultClusterIconGenerator();
        }

        private GMUDefaultClusterIconGenerator iconGeneratorWithImages()
        {
            return new GMUDefaultClusterIconGenerator(
                new NSNumber[] { 50, 100, 250, 500, 1000 },
                new UIImage[]
                {
                    UIImage.FromBundle("m1"),
                    UIImage.FromBundle("m2"),
                    UIImage.FromBundle("m3"),
                    UIImage.FromBundle("m4"),
                    UIImage.FromBundle("m5")
                }
            );
        }

        [Export("removeClusterManager")]
        private void removeClusterManager()
        {
            Console.WriteLine(@"Removing cluster manager. Cluster related markers should be cleared.");
            clusterManager = null;
        }

        [Export("renderer:willRenderMarker:")]
        public void WillRenderMarker(IGMUClusterRenderer renderer, Marker marker)
        {
            if (marker.UserData is POIItem)
            {
                marker.Title = (marker.UserData as POIItem).name;
            }
        }

        private class MapDelegate : MapViewDelegate
        {
            public override bool TappedMarker(MapView mapView, Marker marker)
            {
                if (marker.UserData is POIItem)
                {
                    Console.WriteLine("Did tap marker for cluster item " + (marker.UserData as POIItem).name);
                }
                else
                {
                    Console.WriteLine("Did tap a normal marker");
                }
                return false;
            }

            public override void DidTapAtCoordinate(MapView mapView, CLLocationCoordinate2D coordinate)
            {
                Console.WriteLine(string.Format("Tapped at location: ({0}, {1})", coordinate.Latitude, coordinate.Longitude));
            }
        }
    }
}
