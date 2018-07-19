using System;
using System.IO;
using System.Collections.Generic;

using Android.App;
using Android.Graphics;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Projection;

using Sample.Droid.Views.Base;

namespace Sample.Droid.Views.TileProjection
{
    [Activity(Label = "TileProjectionActivity")]
    public class TileProjectionActivity : BaseActivity
    {
        protected override void StartMap()
        {
            PointTileOverlay pto = new PointTileOverlay();
            pto.AddPoint(new LatLng(0, 0));
            pto.AddPoint(new LatLng(21, -10));
            googleMap.AddTileOverlay(new TileOverlayOptions().InvokeTileProvider(pto));
        }

        private class PointTileOverlay : Java.Lang.Object, ITileProvider
        {
            private List<Android.Gms.Maps.Utils.Geometry.Point> points = new List<Android.Gms.Maps.Utils.Geometry.Point>();
            private static float tileSize = 256;
            private SphericalMercatorProjection projection = new SphericalMercatorProjection(tileSize);
            private static int mScale = 2;
            private int dimension = (int)(mScale * tileSize);

            public Tile GetTile(int x, int y, int zoom)
            {
                Matrix matrix = new Matrix();
                float scale = (float)Math.Pow(2, zoom) * mScale;
                matrix.PostScale(scale, scale);
                matrix.PostTranslate(-x*dimension, -y * dimension);

                Bitmap bitmap = Bitmap.CreateBitmap(dimension, dimension, Bitmap.Config.Argb8888);
                Canvas canvas = new Canvas(bitmap);
                canvas.Matrix = matrix;

                foreach (Android.Gms.Maps.Utils.Geometry.Point p in points)
                {
                    canvas.DrawCircle((float)p.X, (float)p.Y, 1, new Paint());
                }

                using (var baos = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Png, 100, baos);
                    return new Tile(dimension, dimension, baos.ToArray());
                }
            }

            public void AddPoint(LatLng latLng)
            {
                points.Add(projection.ToPoint(latLng));
            }
        }
    }
}
