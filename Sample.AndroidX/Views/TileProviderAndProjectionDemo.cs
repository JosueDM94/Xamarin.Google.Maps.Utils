using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Projection;
using Android.Graphics;
using Java.IO;

namespace Sample.AndroidX
{
    [Activity(Label = "TileProviderAndProjectionDemo")]
    public class TileProviderAndProjectionDemo : BaseDemoActivity
    {
        protected override void startDemo(bool isRestore)
        {
            PointTileOverlay pto = new PointTileOverlay();
            pto.addPoint(new LatLng(0, 0));
            pto.addPoint(new LatLng(21, -10));
            getMap().AddTileOverlay(new TileOverlayOptions().InvokeTileProvider(pto));
        }

        private class PointTileOverlay : Java.Lang.Object, ITileProvider
        {
            private List<Android.Gms.Maps.Utils.Geometry.Point> mPoints = new List<Android.Gms.Maps.Utils.Geometry.Point>();
            private static int mTileSize = 256;
            private SphericalMercatorProjection mProjection = new SphericalMercatorProjection(mTileSize);
            private static int mScale = 2;
            private int mDimension = mScale * mTileSize;

            public void addPoint(LatLng latLng)
            {
                mPoints.Add(mProjection.ToPoint(latLng));
            }

            public Tile GetTile(int x, int y, int zoom)
            {
                Matrix matrix = new Matrix();
                float scale = (float)Math.Pow(2, zoom) * mScale;
                matrix.PostScale(scale, scale);
                matrix.PostTranslate(-x * mDimension, -y * mDimension);

                Bitmap bitmap = Bitmap.CreateBitmap(mDimension, mDimension, Bitmap.Config.Argb8888);
                Canvas c = new Canvas(bitmap);
                c.Matrix = matrix;

                foreach (Android.Gms.Maps.Utils.Geometry.Point p in mPoints)
                {
                    c.DrawCircle((float)p.X, (float)p.Y, 1, new Paint());
                }

                using (var baos = new MemoryStream())
                {
                    bitmap.Compress(Bitmap.CompressFormat.Png, 100, baos);
                    return new Tile(mDimension, mDimension, baos.ToArray());
                }
            }
        }
    }
}
