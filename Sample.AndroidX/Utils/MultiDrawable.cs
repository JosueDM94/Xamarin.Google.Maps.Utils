using System.Collections.Generic;

using Android.Graphics;
using Android.Graphics.Drawables;

namespace Sample.AndroidX.Utils
{
    public class MultiDrawable : Drawable
    {
        private List<Drawable> drawables;

        public MultiDrawable(List<Drawable> drawables)
        {
            this.drawables = drawables;
        }

        public override void Draw(Canvas canvas)
        {
            if (drawables.Count == 1)
            {
                drawables[0].Draw(canvas);
                return;
            }
            int width = Bounds.Width();
            int height = Bounds.Height();

            canvas.Save();
            canvas.ClipRect(0, 0, width, height);

            if (drawables.Count == 2 || drawables.Count == 3)
            {
                // Paint left half
                canvas.Save();
                canvas.ClipRect(0, 0, width / 2, height);
                canvas.Translate(-width / 4, 0);
                drawables[0].Draw(canvas);
                canvas.Restore();
            }
            if (drawables.Count == 2)
            {
                // Paint right half
                canvas.Save();
                canvas.ClipRect(width / 2, 0, width, height);
                canvas.Translate(width / 4, 0);
                drawables[1].Draw(canvas);
                canvas.Restore();
            }
            else
            {
                // Paint top right
                canvas.Save();
                canvas.Scale(.5f, .5f);
                canvas.Translate(width, 0);
                drawables[1].Draw(canvas);

                // Paint bottom right
                canvas.Translate(0, height);
                drawables[2].Draw(canvas);
                canvas.Restore();
            }

            if (drawables.Count >= 4)
            {
                // Paint top left
                canvas.Save();
                canvas.Scale(.5f, .5f);
                drawables[0].Draw(canvas);

                // Paint bottom left
                canvas.Translate(0, height);
                drawables[3].Draw(canvas);
                canvas.Restore();
            }

            canvas.Restore();
        }

        public override void SetAlpha(int alpha)
        {

        }

        public override void SetColorFilter(ColorFilter colorFilter)
        {

        }

        public override int Opacity
        {
            get { return (int)Format.Unknown; }
        }
    }
}