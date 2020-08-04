using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils;
using Android.Graphics;
using Java.Util;

namespace Sample.AndroidX
{
    [Activity(Label = "PolySimplifyDemoActivity")]
    public class PolySimplifyDemoActivity : BaseDemoActivity
    {
        private static string LINE = "elfjD~a}uNOnFN~Em@fJv@tEMhGDjDe@hG^nF??@lA?n@IvAC`Ay@A{@DwCA{CF_EC{CEi@PBTFDJBJ?V?n@?D@?A@?@?F?F?LAf@?n@@`@@T@~@FpA?fA?p@?r@?vAH`@OR@^ETFJCLD?JA^?J?P?fAC`B@d@?b@A\\@`@Ad@@\\?`@?f@?V?H?DD@DDBBDBD?D?B?B@B@@@B@B@B@D?D?JAF@H@FCLADBDBDCFAN?b@Af@@x@@";
        private static string OVAL_POLYGON = "}wgjDxw_vNuAd@}AN{A]w@_Au@kAUaA?{@Ke@@_@C]D[FULWFOLSNMTOVOXO\\I\\CX?VJXJTDTNXTVVLVJ`@FXA\\AVLZBTATBZ@ZAT?\\?VFT@XGZAP";
        private static int ALPHA_ADJUSTMENT = 0x77000000;

        protected override void startDemo(bool isRestore)
        {
            GoogleMap map = getMap();

            // Original line
            List<LatLng> line = PolyUtil.Decode(LINE).ToList();
            map.AddPolyline(new PolylineOptions()
                    .AddAll(new ArrayList(line))
                    .InvokeColor(Color.Black));

            if (!isRestore)
            {
                map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(28.05870, -82.4090), 15));
            }

            List<LatLng> simplifiedLine;

            /*
             * Simplified lines - increasing the tolerance will result in fewer points in the simplified
             * line
             */
            double tolerance = 5; // meters
            simplifiedLine = PolyUtil.Simplify(line, tolerance).ToList();
            map.AddPolyline(new PolylineOptions()
                    .AddAll(new ArrayList(simplifiedLine))
                    .InvokeColor(Color.Red - ALPHA_ADJUSTMENT));

            tolerance = 20; // meters
            simplifiedLine = PolyUtil.Simplify(line, tolerance).ToList();
            map.AddPolyline(new PolylineOptions()
                    .AddAll(new ArrayList(simplifiedLine))
                    .InvokeColor(Color.Green - ALPHA_ADJUSTMENT));

            tolerance = 50; // meters
            simplifiedLine = PolyUtil.Simplify(line, tolerance).ToList();
            map.AddPolyline(new PolylineOptions()
                    .AddAll(new ArrayList(simplifiedLine))
                    .InvokeColor(Color.Magenta - ALPHA_ADJUSTMENT));

            tolerance = 500; // meters
            simplifiedLine = PolyUtil.Simplify(line, tolerance).ToList();
            map.AddPolyline(new PolylineOptions()
                    .AddAll(new ArrayList(simplifiedLine))
                    .InvokeColor(Color.Yellow - ALPHA_ADJUSTMENT));

            tolerance = 1000; // meters
            simplifiedLine = PolyUtil.Simplify(line, tolerance).ToList();
            map.AddPolyline(new PolylineOptions()
                    .AddAll(new ArrayList(simplifiedLine))
                    .InvokeColor(Color.Blue - ALPHA_ADJUSTMENT));


            // Triangle polygon - the polygon should be closed
            List<LatLng> triangle = new List<LatLng>();
            triangle.Add(new LatLng(28.06025, -82.41030));  // Should match last point
            triangle.Add(new LatLng(28.06129, -82.40945));
            triangle.Add(new LatLng(28.06206, -82.40917));
            triangle.Add(new LatLng(28.06125, -82.40850));
            triangle.Add(new LatLng(28.06035, -82.40834));
            triangle.Add(new LatLng(28.06038, -82.40924));
            triangle.Add(new LatLng(28.06025, -82.41030));  // Should match first point

            map.AddPolygon(new PolygonOptions()
                    .AddAll(new ArrayList(triangle))
                    .InvokeFillColor(Color.Blue - ALPHA_ADJUSTMENT)
                    .InvokeStrokeColor(Color.Blue)
                    .InvokeStrokeWidth(5));

            // Simplified triangle polygon
            tolerance = 88; // meters
            List<LatLng> simplifiedTriangle = PolyUtil.Simplify(triangle, tolerance).ToList();
            map.AddPolygon(new PolygonOptions()
                    .AddAll(new ArrayList(simplifiedTriangle))
                    .InvokeFillColor(Color.Yellow - ALPHA_ADJUSTMENT)
                    .InvokeStrokeColor(Color.Yellow)
                    .InvokeStrokeWidth(5));

            // Oval polygon - the polygon should be closed
            List<LatLng> oval = PolyUtil.Decode(OVAL_POLYGON).ToList();
            map.AddPolygon(new PolygonOptions()
                    .AddAll(new ArrayList(oval))
                    .InvokeFillColor(Color.Blue - ALPHA_ADJUSTMENT)
                    .InvokeStrokeColor(Color.Blue)
                    .InvokeStrokeWidth(5));

            // Simplified oval polygon
            tolerance = 10; // meters
            List<LatLng> simplifiedOval = PolyUtil.Simplify(oval, tolerance).ToList();
            map.AddPolygon(new PolygonOptions()
                    .AddAll(new ArrayList(simplifiedOval))
                    .InvokeFillColor(Color.Yellow - ALPHA_ADJUSTMENT)
                    .InvokeStrokeColor(Color.Yellow)
                    .InvokeStrokeWidth(5));
        }
    }
}
