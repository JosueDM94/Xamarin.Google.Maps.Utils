using System;
using CoreGraphics;
using Foundation;
using Google.Maps;
using UIKit;
using Google.Maps.Utils;

namespace Sample.iOS.Views.KML
{
    public partial class KMLViewController : UIViewController
    {
        private double cameraLatitude = 37.4220;
        private double cameraLongitude = -122.0841;

        private MapView mapView;

        public KMLViewController() : base("KMLViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetKML();
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

        private void SetMapView()
        {            
            var camera = CameraPosition.FromCamera(cameraLatitude, cameraLongitude, 17);
            mapView = MapView.FromCamera(CGRect.Empty, camera);
            View = mapView;
        }

        private void SetKML()
        {
            var bundle = NSBundle.MainBundle;
            var path = bundle.PathForResource("KML_Sample", "kml");
            var url = NSUrl.CreateFileUrl(path, null);
            var parser = new GMUKMLParser(url);
            parser.Parse();
            var renderer = new GMUGeometryRenderer(mapView, parser.Placemarks, parser.Styles);
            renderer.Render();
        }
    }
}