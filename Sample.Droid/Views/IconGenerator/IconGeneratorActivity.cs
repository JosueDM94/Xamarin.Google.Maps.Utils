using System;

using Android.App;
using Android.Runtime;
using Android.Graphics;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.UI;

using Sample.Droid.Views.Base;
using Android.Text;
using Android.Text.Style;

namespace Sample.Droid.Views.IconGenerato
{
    [Activity(Label = "IconGeneratorActivity")]
    public class IconGeneratorActivity : BaseActivity
    {        
        protected override void StartMap()
        {
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(-33.8696, 151.2094), 10));

            IconGenerator iconFactory = new IconGenerator(this);
            AddIcon(iconFactory, "Default", new LatLng(-33.8696, 151.2094));

            iconFactory.SetColor(Color.Cyan);
            AddIcon(iconFactory, "Custom color", new LatLng(-33.9360, 151.2070));

            iconFactory.SetRotation(90);
            iconFactory.SetStyle(IconGenerator.StyleRed);
            AddIcon(iconFactory, "Rotated 90 degrees", new LatLng(-33.8858, 151.096));

            iconFactory.SetContentRotation(-90);
            iconFactory.SetStyle(IconGenerator.StylePurple);
            AddIcon(iconFactory, "Rotate=90, ContentRotate=-90", new LatLng(-33.9992, 151.098));

            iconFactory.SetRotation(0);
            iconFactory.SetContentRotation(90);
            iconFactory.SetStyle(IconGenerator.StyleGreen);
            AddIcon(iconFactory, "ContentRotate=90", new LatLng(-33.7677, 151.244));

            iconFactory.SetRotation(0);
            iconFactory.SetContentRotation(0);
            iconFactory.SetStyle(IconGenerator.StyleOrange);
            AddIcon(iconFactory, MakeCharSequence(), new LatLng(-33.77720, 151.12412));
        }

        private void AddIcon(IconGenerator iconFactory, string text, LatLng position)
        {
            MarkerOptions markerOptions = new MarkerOptions().SetIcon(BitmapDescriptorFactory.FromBitmap(iconFactory.MakeIcon(text))).SetPosition(position).Anchor(iconFactory.AnchorU, iconFactory.AnchorV);
            googleMap.AddMarker(markerOptions);
        }

        private string MakeCharSequence()
        {
            string prefix = "Mixing ";
            string suffix = "different fonts";
            string sequence = prefix + suffix;
            SpannableStringBuilder ssb = new SpannableStringBuilder(sequence);
            ssb.SetSpan(new StyleSpan(TypefaceStyle.Italic), 0, prefix.Length, SpanTypes.ExclusiveExclusive);
            ssb.SetSpan(new StyleSpan(TypefaceStyle.Bold), prefix.Length, sequence.Length, SpanTypes.ExclusiveExclusive);
            return ssb.ToString();
        }
    }
}