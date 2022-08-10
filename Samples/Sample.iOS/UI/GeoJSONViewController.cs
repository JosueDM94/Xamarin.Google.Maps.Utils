using CoreGraphics;
using Foundation;
using Google.Maps;
using Google.Maps.Utils;
using UIKit;

namespace Sample.iOS
{
    public class GeoJSONViewController : UIViewController
    {
        private MapView mapView;
        private GMUGeometryRenderer renderer;
        private GMUGeoJSONParser geoJsonParser;

        public override void LoadView()
        {
            var camera = CameraPosition.FromCamera(latitude: -28, longitude: 137, 4);
            mapView = MapView.FromCamera(frame: CGRect.Empty, camera: camera);
            this.View = mapView;

            var path = NSBundle.PathForResourceAbsolute("GeoJSON_sample", "json", NibBundle.BundlePath);
            var url = NSUrl.CreateFileUrl(path, null);
            geoJsonParser = new GMUGeoJSONParser(url);
            geoJsonParser.Parse();

            renderer = new GMUGeometryRenderer(map: mapView, geometries: geoJsonParser.Features);

            renderer.Render();
        }
    }
}