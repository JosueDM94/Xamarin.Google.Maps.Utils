using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Google.Maps;
using Google.Maps.Utils;
using Sample.iOS.Models;
using UIKit;

namespace Sample.iOS
{
    public class CustomMarkerViewController : UIViewController, IGMUClusterRendererDelegate
    {
        double kImageDimension = 30;
        double kCameraLatitude = -33.8;
        double kCameraLongitude = 151.2;

        private MapView mapView;
        private GMUClusterManager clusterManager;

        private Person[] randomPeople()
        {
            return new Person[] { 
                // http://www.flickr.com/photos/sdasmarchives/5036248203/
                new Person(new CLLocationCoordinate2D(-33.8, 151.2), "https://c1.staticflickr.com/5/4125/5036248253_e405cc6961_s.jpg"),
                // http://www.flickr.com/photos/usnationalarchives/4726917149/
                new Person(new CLLocationCoordinate2D(-33.82, 151.1), "https://c2.staticflickr.com/2/1350/4726917149_2a7e7c579e_s.jpg"),
                // http://www.flickr.com/photos/nypl/3111525394/
                new Person(new CLLocationCoordinate2D(-33.9, 151.15), "https://c2.staticflickr.com/4/3101/3111525394_737eaf0dfd_s.jpg"),
                // http://www.flickr.com/photos/smithsonian/2887433330/
                new Person(new CLLocationCoordinate2D(-33.91, 151.05), "https://c2.staticflickr.com/4/3288/2887433330_7e7ed360b1_s.jpg"),
                // http://www.flickr.com/photos/library_of_congress/2179915182/
                new Person(new CLLocationCoordinate2D(-33.7, 151.06), "https://c1.staticflickr.com/3/2405/2179915182_5a0ac98b49_s.jpg"),
                // http://www.flickr.com/photos/nationalmediamuseum/7893552556/
                new Person(new CLLocationCoordinate2D(-33.5, 151.18), "https://c1.staticflickr.com/9/8035/7893552556_3351c8a168_s.jpg"),
                // http://www.flickr.com/photos/sdasmarchives/5036231225/
                new Person(new CLLocationCoordinate2D(-34.0, 151.18), "https://c1.staticflickr.com/5/4125/5036231225_549f804980_s.jpg")
            };
        }

        public override void LoadView()
        {
            var camera = CameraPosition.FromCamera(latitude: kCameraLatitude, longitude: kCameraLongitude, zoom: 10);
            mapView = MapView.FromCamera(frame: CGRect.Empty, camera: camera);
            this.View = mapView;

            // Set up the cluster manager with default icon generator and renderer.
            var iconGenerator = new GMUDefaultClusterIconGenerator();
            var algorithm = new GMUNonHierarchicalDistanceBasedAlgorithm();
            var renderer = new ClusterRenderer(mapView: mapView, iconGenerator: iconGenerator) { Delegate = this };
            clusterManager = new GMUClusterManager(mapView: mapView, algorithm: algorithm, renderer: renderer);

            // Add people to the cluster manager.
            foreach(Person person in randomPeople())
                clusterManager.AddItem(person);

            // Call cluster() after items have been added to perform the clustering and rendering on map.
            clusterManager.Cluster();
        }

        [Export("renderer:willRenderMarker:")]
        public void WillRenderMarker(IGMUClusterRenderer renderer, Marker marker)
        {
            if (marker.UserData is Person person)
            {
                marker.Title = person.imageUrl;
                marker.Icon = ImageForItem(person);
                marker.GroundAnchor = new CGPoint(0.5, 0.5);
            }
            else if (marker.UserData is GMUStaticCluster)
            {
                marker.Icon = ImageForCluster(marker.UserData as GMUStaticCluster);
            }
        }

        // Returns an image representing the cluster item marker.
        private UIImage ImageForItem(Person person)
        {
            if (person.cacheImage == null)
            {
                // Note: synchronously download and resize the image. Ideally the image should either be cached
                // already or the download should happens asynchronously.
                person.cacheImage = ImageWithContentsOfURL(person.imageUrl, new CGSize(kImageDimension, kImageDimension));
            }
            return person.cacheImage;
        }

        // Returns an image representing the cluster marker. Only takes a maximum of 4
        // items in the cluster to create the mashed up image.
        private UIImage ImageForCluster(GMUStaticCluster cluster)
        {
            var items = cluster.Items;
            List<UIImage> images = new List<UIImage>();
            for (int i = 0; i < items.Count(); i++)
            {
                images.Add(ImageForItem(items[i] as Person));
                if (i >= 4)
                    break;
            }
            return ImageFromImages(images, new CGSize(kImageDimension * 2, kImageDimension * 2));
        }

        // Returns a new image with half the width of the original.
        private UIImage HalfOfImage(UIImage image)
        {
            nfloat scale = image.CurrentScale;
            nfloat width = image.Size.Width * scale;
            nfloat height = image.Size.Height * scale;
            CGRect rect = new CGRect(width / 4, 0, width / 2, height);
            CGImage imageRef = image.CGImage.WithImageInRect(rect);
            UIImage newImage = UIImage.FromImage(imageRef, scale, image.Orientation);
            return newImage;
        }

        // Mashes up the images.
        private UIImage ImageFromImages(List<UIImage> images, CGSize size)
        {
            if (images.Count <= 1)
                return images.FirstOrDefault();

            UIGraphics.BeginImageContextWithOptions(size, true, 0);
            if (images.Count == 2 || images.Count == 3)
            {
                // Draw left half.
                images[0].Draw(new CGRect(-size.Width / 4, 0, size.Width, size.Height));
            }

            if (images.Count == 2)
            {
                // Draw right half.
                UIImage halfOfImage = HalfOfImage(images[1]);
                halfOfImage.Draw(new CGRect(size.Width / 2, 0, size.Width, size.Height));
            }
            else
            {
                // Draw top right quadrant.
                images[1].Draw(new CGRect(size.Width / 2, 0, size.Width, size.Height));
                // Draw bottom right quadrant.
                images[2].Draw(new CGRect(size.Width / 2, size.Height / 2, size.Width / 2, size.Height / 2));
            }
            if (images.Count >= 4)
            {
                // Draw top left quadrant.
                images[0].Draw(new CGRect(0, 0, size.Width / 2, size.Height / 2));
                // Draw bottom left quadrant.
                images[3].Draw(new CGRect(0, size.Height / 2, size.Width / 2, size.Height / 2));
            }
            UIImage newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return newImage;
        }

        // Downloads and resize an image.
        private UIImage ImageWithContentsOfURL(string url, CGSize size)
        {
            NSData data = NSData.FromUrl(new NSUrl(url));
            UIImage image = UIImage.LoadFromData(data);
            UIGraphics.BeginImageContextWithOptions(size, true, 0);
            image.Draw(new CGRect(0, 0, size.Width, size.Height));
            UIImage newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return newImage;
        }


        private class ClusterRenderer : GMUDefaultClusterRenderer
        {
            public ClusterRenderer(MapView mapView, IGMUClusterIconGenerator iconGenerator) : base(mapView, iconGenerator)
            {
            }

            // Show as a cluster for clusters whose size is >= 2.
            public override bool ShouldRenderAsCluster(IGMUCluster cluster, float zoom)
            {
                return cluster.Count >= 2;
            }
        }
    }
}
