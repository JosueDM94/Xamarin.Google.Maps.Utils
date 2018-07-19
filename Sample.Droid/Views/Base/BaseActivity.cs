using Android.OS;
using Android.App;
using Android.Gms.Maps;
using Android.Support.V4.App;

namespace Sample.Droid.Views.Base
{
    [Activity(Label = "BaseActivity")]
    public abstract class BaseActivity : FragmentActivity, IOnMapReadyCallback
    {
        protected GoogleMap googleMap { get; private set; }
        protected MapFragment mapFragment { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(LayoutId());
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnResume()
        {
            base.OnResume();
            InitElements();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            if (googleMap == null)
                return;
            
            this.googleMap = googleMap;
            StartMap();
        }

        private void InitElements()
        {
            mapFragment = FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map);
            mapFragment.GetMapAsync(this);
        }

        protected abstract void StartMap();

        protected virtual int LayoutId()
        {
            return Resource.Layout.MapLayout;
        }
    }
}