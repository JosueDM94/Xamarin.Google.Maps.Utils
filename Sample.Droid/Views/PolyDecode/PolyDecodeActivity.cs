using System.Linq;

using Java.Util;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils;

using Sample.Droid.Views.Base;

namespace Sample.Droid.Views.PolyDecode
{
    [Activity(Label = "PolyDecodeActivity")]
    public class PolyDecodeActivity : BaseActivity
    {
        private static string LINE = "rvumEis{y[}DUaBGu@EqESyCMyAGGZGdEEhBAb@DZBXCPGP]Xg@LSBy@E{@SiBi@wAYa@AQGcAY]I]KeBm@_Bw@cBu@ICKB}KiGsEkCeEmBqJcFkFuCsFuCgB_AkAi@cA[qAWuAKeB?uALgB\\eDx@oBb@eAVeAd@cEdAaCp@s@PO@MBuEpA{@R{@NaAHwADuBAqAGE?qCS[@gAO{Fg@qIcAsCg@u@SeBk@aA_@uCsAkBcAsAy@AMGIw@e@_Bq@eA[eCi@QOAK@O@YF}CA_@Ga@c@cAg@eACW@YVgDD]Nq@j@}AR{@rBcHvBwHvAuFJk@B_@AgAGk@UkAkBcH{@qCuAiEa@gAa@w@c@o@mA{Ae@s@[m@_AaCy@uB_@kAq@_Be@}@c@m@{AwAkDuDyC_De@w@{@kB_A}BQo@UsBGy@AaA@cLBkCHsBNoD@c@E]q@eAiBcDwDoGYY_@QWEwE_@i@E}@@{BNaA@s@EyB_@c@?a@F}B\\iCv@uDjAa@Ds@Bs@EyAWo@Sm@a@YSu@c@g@Mi@GqBUi@MUMMMq@}@SWWM]C[DUJONg@hAW\\QHo@BYIOKcG{FqCsBgByAaAa@gA]c@I{@Gi@@cALcEv@_G|@gAJwAAUGUAk@C{Ga@gACu@A[Em@Sg@Y_AmA[u@Oo@qAmGeAeEs@sCgAqDg@{@[_@m@e@y@a@YIKCuAYuAQyAUuAWUaA_@wBiBgJaAoFyCwNy@cFIm@Bg@?a@t@yIVuDx@qKfA}N^aE@yE@qAIeDYaFBW\\eBFkANkANWd@gALc@PwAZiBb@qCFgCDcCGkCKoC`@gExBaVViDH}@kAOwAWe@Cg@BUDBU`@sERcCJ{BzFeB";

        protected override void StartMap()
        {
            var decodePath = PolyUtil.Decode(LINE).ToList();
            googleMap.AddPolyline(new PolylineOptions().AddAll(new ArrayList(decodePath)));
            googleMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(-33.8256, 151.2395), 12));
        }
    }
}
