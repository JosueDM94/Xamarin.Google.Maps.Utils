using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Data.Kml;

using Sample.Droid.Views.Base;

namespace Sample.Droid.Views.Kml
{
    [Activity(Label = "KmlActivity")]
    public class KmlActivity : BaseActivity
    {
        private GoogleMap mMap;

        protected override int LayoutId()
        {
            return Resource.Layout.KmlLayout;
        }

        protected override void StartMap()
        {
            try{
                mMap = googleMap;
                //RetrieveFileFromResource();
                RetrieveFileFromUrl();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception caught"+e.Message);
            }
        }

        private void RetrieveFileFromResource()
        {
            try
            {
                KmlLayer kmlLayer = new KmlLayer(mMap, Resource.Raw.campus, this);
                kmlLayer.AddLayerToMap();
                MoveCameraToKml(kmlLayer);
            }
            catch (Exception e)
            {                
                Console.WriteLine(e.StackTrace);
            }
        }

        private void RetrieveFileFromUrl()
        {
            DownloadKmlFile(GetString(Resource.String.kml_url));
        }

        private void MoveCameraToKml(KmlLayer kmlLayer)
        {
            //Retrieve the first container in the KML layer
            KmlContainer container = kmlLayer.Containers.ToEnumerable<KmlContainer>().FirstOrDefault();
            //Retrieve a nested container within the first container
            container = container.Containers.ToEnumerable<KmlContainer>().FirstOrDefault();
            //Retrieve the first placemark in the nested container
            KmlPlacemark placemark = container.Placemarks.ToEnumerable<KmlPlacemark>().FirstOrDefault();
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
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLngBounds(builder.Build(), width, height, 1));
        }

        private async void DownloadKmlFile(string url)
        {
            await Task.Factory.StartNew(() =>
            {
                using (var Client = new HttpClient())
                {
                    var response = Client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var kmlLayer = new KmlLayer(googleMap, response.Content.ReadAsStreamAsync().Result,this);
                        this.RunOnUiThread(() =>
                        {                            
                            kmlLayer.AddLayerToMap();
                            MoveCameraToKml(kmlLayer);
                        });
                    }
                }
            }).ConfigureAwait(false);
        }
    }
}
