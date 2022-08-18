using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace Sample.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnClickListener
    {
        private ViewGroup mListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);

            if (string.IsNullOrEmpty(GetString(Resource.String.maps_api_key)))
            {
                Toast.MakeText(this, "Add your own API key in demo/secure.properties as MAPS_API_KEY=YOUR_API_KEY", ToastLength.Long).Show();
                return;
            }

            mListView = FindViewById<LinearLayout>(Resource.Id.list);

            addDemo("Clustering", new MyClass(typeof(ClusteringDemoActivity)));
            addDemo("Clustering: Custom Look", new MyClass(typeof(CustomMarkerClusteringDemoActivity)));
            addDemo("Clustering: 2K markers", new MyClass(typeof(BigClusteringDemoActivity)));
            addDemo("Clustering: 20K only visible markers", new MyClass(typeof(VisibleClusteringDemoActivity)));
            addDemo("Clustering: ViewModel", new MyClass(typeof(ClusteringViewModelDemoActivity)));
            addDemo("PolyUtil.decode", new MyClass(typeof(PolyDecodeDemoActivity)));
            addDemo("PolyUtil.simplify", new MyClass(typeof(PolySimplifyDemoActivity)));
            addDemo("IconGenerator", new MyClass(typeof(IconGeneratorDemoActivity)));
            addDemo("SphericalUtil.computeDistanceBetween", new MyClass(typeof(DistanceDemoActivity)));
            addDemo("Generating tiles", new MyClass(typeof(TileProviderAndProjectionDemo)));
            addDemo("Heatmaps", new MyClass(typeof(HeatmapsDemoActivity)));
            addDemo("Heatmaps with Places API", new MyClass(typeof(HeatmapsPlacesDemoActivity)));
            addDemo("GeoJSON Layer", new MyClass(typeof(GeoJsonDemoActivity)));
            addDemo("KML Layer Overlay", new MyClass(typeof(KmlDemoActivity)));
            addDemo("Multi Layer", new MyClass(typeof(MultiLayerDemoActivity)));
        }

        private void addDemo(string demoName, Java.Lang.Object activityClass)
        {
            Button b = new Button(this);
            ViewGroup.LayoutParams layoutParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            b.LayoutParameters = layoutParams;
            b.Text = demoName;
            b.Tag = activityClass;
            b.SetOnClickListener(this);
            mListView.AddView(b);
        }

        public void OnClick(View v)
        {
            Type activityClass = (v.Tag as MyClass).Type;
            StartActivity(new Intent(this, activityClass));            
        }

        public class MyClass : Java.Lang.Object
        {
            public Type Type;

            public MyClass(Type type)
            {
                Type = type;
            }
        }
    }
}