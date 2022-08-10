using CoreGraphics;
using Foundation;
using Google.Maps;
using Google.Maps.Utils;
using UIKit;

namespace Sample.iOS
{
    public class KMLViewController : UIViewController
    {
        private MapView mapView;
        private GMUGeometryRenderer renderer;
        private GMUKMLParser kmlParser;

        public override void LoadView()
        {
            var camera = CameraPosition.FromCamera(latitude: 37.4220, longitude: -122.0841, 17);
            mapView = MapView.FromCamera(frame: CGRect.Empty, camera: camera);
            this.View = mapView;

            var path = NSBundle.PathForResourceAbsolute("KML_Sample", "kml", NibBundle.BundlePath);
            var url = NSUrl.CreateFileUrl(path, null);
            kmlParser = new GMUKMLParser(url);
            kmlParser.Parse();

            renderer = new GMUGeometryRenderer(map: mapView,
                                               geometries: kmlParser.Placemarks,
                                               styles: kmlParser.Styles);

            renderer.Render();
        }
    }
}