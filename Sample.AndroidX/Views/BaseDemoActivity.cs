using Android.Gms.Maps;
using Android.OS;
using AndroidX.Fragment.App;

namespace Sample.AndroidX
{
    public abstract class BaseDemoActivity : FragmentActivity, IOnMapReadyCallback
    {
        private GoogleMap mMap;
        private bool mIsRestore;

        protected virtual int getLayoutId()
        {
            return Resource.Layout.map;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            mIsRestore = savedInstanceState != null;
            SetContentView(getLayoutId());
            setUpMap();
        }

        void IOnMapReadyCallback.OnMapReady(GoogleMap map)
        {
            if (mMap != null)
            {
                return;
            }
            mMap = map;
            startDemo(mIsRestore);
        }

        private void setUpMap()
        {
            ((SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map)).GetMapAsync(this);
        }

        /**
         * Run the demo-specific code.
         */
        protected abstract void startDemo(bool isRestore);

        protected GoogleMap getMap()
        {
            return mMap;
        }
    }
}
