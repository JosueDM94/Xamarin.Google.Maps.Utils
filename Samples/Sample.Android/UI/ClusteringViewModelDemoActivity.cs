using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils.Clustering;
using Android.OS;
using Android.Util;
using Android.Widget;
using AndroidX.Lifecycle;
using Org.Json;

namespace Sample.Android
{
    [Activity(Label = "ClusteringViewModelDemoActivity")]
    public class ClusteringViewModelDemoActivity : BaseDemoActivity
    {
        private ClusterManager mClusterManager;
        private ClusteringViewModel mViewModel;

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mViewModel = new ViewModelProvider(this).Get(Java.Lang.Class.FromType(typeof(ClusteringViewModel))) as ClusteringViewModel;
            if (savedInstanceState == null)
            {
                try
                {
                    mViewModel.readItems(Resources);
                }
                catch (JSONException)
                {
                    Toast.MakeText(this, "Problem reading list of markers.", ToastLength.Long).Show();
                }
            }
        }

        protected override void startDemo(bool isRestore)
        {
            if (!isRestore)
            {
                getMap().MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(51.503186, -0.126446), 10));
            }

            DisplayMetrics metrics = new DisplayMetrics();
#pragma warning disable CS0618 // Type or member is obsolete
            WindowManager. DefaultDisplay.GetMetrics(metrics);
#pragma warning restore CS0618 // Type or member is obsolete
            //mViewModel.getAlgorithm().UpdateViewSize(metrics.WidthPixels, metrics.HeightPixels);

            mClusterManager = new ClusterManager(this, getMap());
            mClusterManager.Algorithm = mViewModel.getAlgorithm();

            getMap().SetOnCameraIdleListener(mClusterManager);
        }
    }
}
