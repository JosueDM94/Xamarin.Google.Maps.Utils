using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.UI;
using Android.Graphics;
using Android.Text;
using StyleSpan = Android.Text.Style.StyleSpan;

namespace Sample.Android
{
    [Activity(Label = "IconGeneratorDemoActivity")]
    public class IconGeneratorDemoActivity : BaseDemoActivity
    {
        protected override void startDemo(bool isRestore)
        {
            if (!isRestore)
            {
                getMap().MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(-33.8696, 151.2094), 10));
            }

            IconGenerator iconFactory = new IconGenerator(this);
            addIcon(iconFactory, "Default", new LatLng(-33.8696, 151.2094));

            iconFactory.SetColor(Color.Cyan);
            addIcon(iconFactory, "Custom color", new LatLng(-33.9360, 151.2070));

            iconFactory.SetRotation(90);
            iconFactory.SetStyle(IconGenerator.StyleRed);
            addIcon(iconFactory, "Rotated 90 degrees", new LatLng(-33.8858, 151.096));

            iconFactory.SetContentRotation(-90);
            iconFactory.SetStyle(IconGenerator.StylePurple);
            addIcon(iconFactory, "Rotate=90, ContentRotate=-90", new LatLng(-33.9992, 151.098));

            iconFactory.SetRotation(0);
            iconFactory.SetContentRotation(90);
            iconFactory.SetStyle(IconGenerator.StyleGreen);
            addIcon(iconFactory, "ContentRotate=90", new LatLng(-33.7677, 151.244));

            iconFactory.SetRotation(0);
            iconFactory.SetContentRotation(0);
            iconFactory.SetStyle(IconGenerator.StyleOrange);
            addIcon(iconFactory, makeCharSequence(), new LatLng(-33.77720, 151.12412));
        }

        private void addIcon(IconGenerator iconFactory, string text, LatLng position)
        {
            MarkerOptions markerOptions = new MarkerOptions();
            markerOptions.SetIcon(BitmapDescriptorFactory.FromBitmap(iconFactory.MakeIcon(text)));
            markerOptions.SetPosition(position);
            markerOptions.Anchor(iconFactory.AnchorU, iconFactory.AnchorV);

            getMap().AddMarker(markerOptions);
        }

        private string makeCharSequence()
        {
            string prefix = "Mixing ";
            string suffix = "different fonts";
            string sequence = prefix + suffix;
            SpannableStringBuilder ssb = new SpannableStringBuilder(sequence);
            ssb.SetSpan(new StyleSpan(TypefaceStyle.Italic), 0, prefix.Length, SpanTypes.ExclusiveExclusive);
            ssb.SetSpan(new StyleSpan(TypefaceStyle.Bold), prefix.Length, sequence.Length, SpanTypes.ExclusiveExclusive);
            return ssb.SubSequence(0, ssb.Length());
        }
    }
}
