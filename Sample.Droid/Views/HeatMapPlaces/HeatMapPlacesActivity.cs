using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Runtime;
using Android.Content;
using Android.Graphics;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils;
using Android.Views.InputMethods;
using Android.Gms.Maps.Utils.HeatMaps;

using Sample.Droid.Models;
using Sample.Droid.Views.Base;

namespace Sample.Droid.Views.HeatMapPlaces
{
    [Activity(Label = "HeatMapPlacesActivity")]
    public class HeatMapPlacesActivity : BaseActivity,EditText.IOnEditorActionListener
    {
        private GoogleMap mMap = null;

        private LatLng SYDNEY = new LatLng(-33.873651, 151.2058896);

        /*
         * The base URL for the radar search request.
         */
        private static string PLACES_API_BASE = "https://maps.googleapis.com/maps/api/place";

        /*
         * The options required for the radar search.
         */
        private static string TYPE_RADAR_SEARCH = "/radarsearch";
        private static string OUT_JSON = "/json";

        /*
         * Places API server key.
         */
        private static string API_KEY = ""; // TODO place your own here!

        public enum HeatmapColors
        {
            RED = 0,
            BLUE = 1,
            GREEN = 2,
            PINK = 3,
            GREY = 4
        }
        /*
         * The colors to be used for the different heatmap layers.
         */
        public Dictionary<HeatmapColors, Color> HEATMAP_COLORS = new Dictionary<HeatmapColors, Color>();

        private int MAX_CHECKBOXES = 5;

        /*
         * The search radius which roughly corresponds to the radius of the results
         * from the radar search in meters.
         */
        public static int SEARCH_RADIUS = 8000;

        /*
         * Stores the TileOverlay corresponding to each of the keywords that have been searched for.
         */
        private Dictionary<string, TileOverlay> mOverlays = new Dictionary<string, TileOverlay>();

        /*
         * A layout containing checkboxes for each of the heatmaps rendered.
         */
        private LinearLayout checkboxLayout;

        /*
         * The number of overlays rendered so far.
         */
        private int mOverlaysRendered = 0;

        /*
         * The number of overlays that have been inputted so far.
         */
        private int mOverlaysInput = 0;

        private Button btnGo;

        protected override void OnCreate(Bundle savedInstanceState)
        {            
            base.OnCreate(savedInstanceState);
            btnGo = FindViewById<Button>(Resource.Id.btnGo);
            HEATMAP_COLORS.Add(HeatmapColors.RED, Color.Rgb(238, 44, 44));
            HEATMAP_COLORS.Add(HeatmapColors.BLUE, Color.Rgb(60, 80, 255));
            HEATMAP_COLORS.Add(HeatmapColors.GREEN, Color.Rgb(20, 170, 50));
            HEATMAP_COLORS.Add(HeatmapColors.PINK, Color.Rgb(255, 80, 255));
            HEATMAP_COLORS.Add(HeatmapColors.GREY, Color.Rgb(100, 100, 100));
        }

        protected override void OnStart()
        {
            base.OnStart();
            btnGo.Click += BtnGo_Click;
        }

        protected override void OnStop()
        {
            base.OnStop();
            btnGo.Click -= BtnGo_Click;
        }

        protected override void StartMap()
        {
            EditText editText = FindViewById<EditText>(Resource.Id.input_text);
            editText.SetOnEditorActionListener(this);
            checkboxLayout = FindViewById<LinearLayout>(Resource.Id.checkboxes);
            SetUpMap();
        }

        protected override int LayoutId()
        {
            return Resource.Layout.PlacesLayout;
        }

        private void SetUpMap()
        {
            if(mMap == null)
            {
                mMap = googleMap;
                if(mMap != null)
                {
                    mMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(SYDNEY, 11));
                    mMap.AddCircle(new CircleOptions().InvokeCenter(SYDNEY).InvokeRadius(SEARCH_RADIUS * 1.2).InvokeStrokeColor(Color.Red).InvokeStrokeWidth(4));
                }
            }
        }

        private class PointsKeywords
        {
            public List<LatLng> points { get; set; }
            public string keyword;

            public PointsKeywords(List<LatLng> points, string keyword)
            {
                this.points = points;
                this.keyword = keyword;
            }
        }

        private void MakeOverlay(string keyword)
        {
            var points = new PointsKeywords(GetPoints(keyword), keyword);
            if(points.points.Count != 0)
            {
                if(mOverlays.Count < MAX_CHECKBOXES)
                {
                    MakeCheckBox(points.keyword);
                    HeatmapTileProvider provider = new HeatmapTileProvider.Builder().Data(points.points).Gradient(MakeGradiant(HEATMAP_COLORS[(HeatmapColors)mOverlaysRendered])).Build();
                    TileOverlay overlay = googleMap.AddTileOverlay(new TileOverlayOptions().InvokeTileProvider(provider));
                    mOverlays.Add(points.keyword, overlay);
                }
                mOverlaysRendered++;
                if(mOverlaysRendered == mOverlaysInput)
                {
                    ProgressBar progressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
                    progressBar.Visibility = ViewStates.Gone;
                }
            }
            else
            {
                ProgressBar progressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
                progressBar.Visibility = ViewStates.Gone;
                Toast.MakeText(this, "No results for this query :(", ToastLength.Short).Show();
            }
        }

        private Gradient MakeGradiant(int color)
        {
            int[] colors = { color };
            float[] starPoints = { 1.0f };
            return new Gradient(colors, starPoints);
        }

        public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
            bool handled = false;
            if (actionId == ImeAction.Go)
            {
                Submit();
                handled = true;
            }
            return handled;
        }

        public void Submit()
        {
            if ("YOUR_KEY_HERE".Equals(API_KEY))
            {
                Toast.MakeText(this, "Please sign up for a Places API key and add it to HeatmapsPlacesDemoActivity.API_KEY",ToastLength.Long).Show();
                return;
            }
            EditText editText = FindViewById<EditText>(Resource.Id.input_text);
            String keyword = editText.Text;
            if (mOverlays.ContainsKey(keyword))
            {
                Toast.MakeText(this, "This keyword has already been inputted :(", ToastLength.Short).Show();
            }
            else if (mOverlaysRendered == MAX_CHECKBOXES)
            {
                Toast.MakeText(this, "You can only input " + MAX_CHECKBOXES + " keywords. :(", ToastLength.Short).Show();
            }
            else if (keyword.Length != 0)
            {
                mOverlaysInput++;
                ProgressBar progressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
                progressBar.Visibility = ViewStates.Visible;
                editText.Text = "";
                MakeOverlay(keyword);
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(editText.WindowToken, 0);
            }
        }

        private List<LatLng> GetPoints(String keyword)
        {
            Dictionary<string, LatLng> results = new Dictionary<string, LatLng>();

            // Calculate four equidistant points around Sydney to use as search centers
            //   so that four searches can be done.
            List<LatLng> searchCenters = new List<LatLng>(4);
            for (int heading = 45; heading < 360; heading += 90)
            {
                searchCenters.Add(SphericalUtil.ComputeOffset(SYDNEY, SEARCH_RADIUS / 2, heading));
            }

            for (int j = 0; j < 4; j++)
            {
                var result = GetJsonPlaces(keyword, searchCenters[j]).Result;
                try
                {
                    for (int i = 0; i < result.Place.Count; i++)
                    {
                        if(!results.ContainsKey(result.Place[i].Id))
                        {
                            results.Add(result.Place[i].Id, new LatLng(result.Place[i].Geometry.Location.Latitude,result.Place[i].Geometry.Location.Longitude));
                        }
                    }
                }
                catch (Exception)
                {
                    Toast.MakeText(this, "Cannot process JSON results", ToastLength.Short).Show();
                }
            }
            return results.Values.ToList();
        }

        private async Task<PlacesWrapper> GetJsonPlaces(String keyword, LatLng location)
        {
            try
            {
                PlacesWrapper result = null;
                await Task.Factory.StartNew(() =>
                {
                    using (var Client = new HttpClient())
                    {
                        var url = PLACES_API_BASE + TYPE_RADAR_SEARCH + OUT_JSON + "?location=" + location.Latitude + "," + location.Longitude + "&radius=" + (SEARCH_RADIUS / 2) + "&sensor=false" + "&key=" + API_KEY + "&keyword=" + keyword.Replace(" ", "%20");
                        var response = Client.GetAsync(url).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            result =  JsonConvert.DeserializeObject<PlacesWrapper>(response.Content.ReadAsStringAsync().Result);
                        }
                    }
                }).ConfigureAwait(false);
                return result;
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Error connecting to Places API", ToastLength.Short).Show();
                return null;
            }
        }

        private void MakeCheckBox(string keyword)
        {
            checkboxLayout.Visibility = ViewStates.Visible;
            CheckBox checkbox = new CheckBox(this);
            checkbox.Text = keyword;
            checkbox.SetTextColor(HEATMAP_COLORS[(HeatmapColors)mOverlaysRendered]);
            checkbox.Checked = true;
            checkbox.CheckedChange -= Checkbox_CheckedChange;
            checkbox.CheckedChange += Checkbox_CheckedChange;
            checkbox.Tag = keyword;
            checkboxLayout.AddView(checkbox);
        }

        void Checkbox_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            // Text is the keyword
            TileOverlay overlay = mOverlays[c.Tag.ToString()];
            if (overlay != null)
            {
                overlay.Visible = c.Checked;
            }
        }

        void BtnGo_Click(object sender, EventArgs e)
        {
            Submit();
        }
    }
}