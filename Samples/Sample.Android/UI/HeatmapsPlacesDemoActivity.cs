using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Maps.Utils;
using Android.Gms.Maps.Utils.HeatMaps;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.IO;
using Java.Net;
using Org.Json;

namespace Sample.Android
{
    [Activity(Label = "HeatmapsPlacesDemoActivity")]
    public class HeatmapsPlacesDemoActivity : BaseDemoActivity, TextView.IOnEditorActionListener, View.IOnClickListener
    {
        private LatLng SYDNEY = new LatLng(-33.873651, 151.2058896);

        /**
         * The base URL for the radar search request.
         */
        private static String PLACES_API_BASE = "https://maps.googleapis.com/maps/api/place";

        /**
         * The options required for the radar search.
         */
        private static string TYPE_RADAR_SEARCH = "/radarsearch";
        private static string OUT_JSON = "/json";

        /**
         * Places API server key.
         */
        private static string API_KEY = "YOUR_KEY_HERE"; // TODO place your own here!

        /**
         * The colors to be used for the different heatmap layers.
         */
        public Dictionary<HeatmapColors, Color> HEATMAP_COLORS = new Dictionary<HeatmapColors, Color>
        {
            { HeatmapColors.RED, Color.Rgb(238, 44, 44) },
            { HeatmapColors.BLUE, Color.Rgb(60, 80, 255) },
            { HeatmapColors.GREEN, Color.Rgb(20, 170, 50) },
            { HeatmapColors.PINK, Color.Rgb(255, 80, 255) },
            { HeatmapColors.GREY, Color.Rgb(100, 100, 100) }
        };

        public enum HeatmapColors
        {
            RED = 0,
            BLUE = 1,
            GREEN = 2,
            PINK = 3,
            GREY = 4
        }

        private static int MAX_CHECKBOXES = 5;

        /**
         * The search radius which roughly corresponds to the radius of the results
         * from the radar search in meters.
         */
        public static int SEARCH_RADIUS = 8000;

        /**
         * Stores the TileOverlay corresponding to each of the keywords that have been searched for.
         */
        private Dictionary<string, TileOverlay> mOverlays = new Dictionary<string, TileOverlay>();

        /**
         * A layout containing checkboxes for each of the heatmaps rendered.
         */
        private LinearLayout mCheckboxLayout;

        /**
         * The number of overlays rendered so far.
         */
        private int mOverlaysRendered = 0;

        /**
         * The number of overlays that have been inputted so far.
         */
        private int mOverlaysInput = 0;

        protected override int getLayoutId()
        {
            return Resource.Layout.places_demo;
        }

        protected override void startDemo(bool isRestore)
        {
            EditText editText = FindViewById<EditText>(Resource.Id.input_text);
            editText.SetOnEditorActionListener(this);

            mCheckboxLayout = FindViewById<LinearLayout>(Resource.Id.checkboxes);
            GoogleMap map = getMap();
            if (!isRestore)
            {
                map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(SYDNEY, 11));
            }
            // Add a circle around Sydney to roughly encompass the results
            map.AddCircle(new CircleOptions()
                    .InvokeCenter(SYDNEY)
                    .InvokeRadius(SEARCH_RADIUS * 1.2)
                    .InvokeStrokeColor(Color.Red)
                    .InvokeStrokeWidth(4));
        }

        /**
         * Takes the input from the user and generates the required heatmap.
         * Called when a search query is submitted
         */
        public void submit(View view)
        {
            if ("YOUR_KEY_HERE".Equals(API_KEY))
            {
                Toast.MakeText(this, "Please sign up for a Places API key and add it to HeatmapsPlacesDemoActivity.API_KEY", ToastLength.Long).Show();
                return;
            }
            EditText editText = FindViewById<EditText>(Resource.Id.input_text);
            string keyword = editText.Text;
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
                MakeOverlayTask(keyword);
                editText.Text = "";

                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(editText.WindowToken, 0);
            }
        }

        /**
         * Makes four radar search requests for the given keyword, then parses the
         * json output and returns the search results as a collection of LatLng objects.
         *
         * @param keyword A string to use as a search term for the radar search
         * @return Returns the search results from radar search as a collection
         * of LatLng objects.
         */
        private List<LatLng> getPoints(string keyword)
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
                string jsonResults = getJsonPlaces(keyword, searchCenters[j]);
                try
                {
                    // Create a JSON object hierarchy from the results
                    JSONObject jsonObj = new JSONObject(jsonResults);
                    JSONArray pointsJsonArray = jsonObj.GetJSONArray("results");

                    // Extract the Place descriptions from the results
                    for (int i = 0; i < pointsJsonArray.Length(); i++)
                    {
                        if (!results.ContainsKey(pointsJsonArray.GetJSONObject(i).GetString("id")))
                        {
                            JSONObject location = pointsJsonArray.GetJSONObject(i)
                                    .GetJSONObject("geometry").GetJSONObject("location");
                            results.Add(pointsJsonArray.GetJSONObject(i).GetString("id"),
                                    new LatLng(location.GetDouble("lat"),
                                            location.GetDouble("lng")));
                        }
                    }
                }
                catch (JSONException)
                {
                    Toast.MakeText(this, "Cannot process JSON results", ToastLength.Short).Show();
                }
            }
            return results.Values.ToList();
        }

        /**
         * Makes a radar search request and returns the results in a json format.
         *
         * @param keyword  The keyword to be searched for.
         * @param location The location the radar search should be based around.
         * @return The results from the radar search request as a json
         */
        private string getJsonPlaces(string keyword, LatLng location)
        {
            HttpURLConnection conn = null;
            StringBuilder jsonResults = new StringBuilder();
            try
            {
                URL url = new URL(
                        PLACES_API_BASE + TYPE_RADAR_SEARCH + OUT_JSON
                        + "?location=" + location.Latitude + "," + location.Longitude
                        + "&radius=" + (SEARCH_RADIUS / 2)
                        + "&sensor=false"
                        + "&key=" + API_KEY
                        + "&keyword=" + keyword.Replace(" ", "%20")
                );
                conn = (HttpURLConnection)url.OpenConnection();
                InputStreamReader @in = new InputStreamReader(conn.InputStream);

                // Load the results into a StringBuilder
                int read;
                char[] buff = new char[1024];
                while ((read = @in.Read(buff)) != -1) {
                    jsonResults.Append(buff, 0, read);
                }
            }
            catch (MalformedURLException)
            {
                Toast.MakeText(this, "Error processing Places API URL", ToastLength.Short).Show();
                return null;
            }
            catch (IOException)
            {
                Toast.MakeText(this, "Error connecting to Places API", ToastLength.Short).Show();
                return null;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Disconnect();
                }
            }
            return jsonResults.ToString();
        }

        /**
         * Creates check box for a given search term
         *
         * @param keyword the search terms associated with the check box
         */
        private void makeCheckBox(string keyword)
        {
            mCheckboxLayout.Visibility = ViewStates.Visible;

            // Make new checkbox
            CheckBox checkBox = new CheckBox(this);
            checkBox.Text = keyword;
            checkBox.SetTextColor(HEATMAP_COLORS[(HeatmapColors)mOverlaysRendered]);
            checkBox.Checked = true;
            checkBox.SetOnClickListener(this);
            checkBox.Tag = keyword;
            mCheckboxLayout.AddView(checkBox);
        }

        /**
         * Async task, because finding the points cannot be done on the main thread, while adding
         * the overlay must be done on the main thread.
         */
        private void MakeOverlayTask(string keyword)
        {
            var pointsKeywords = new PointsKeywords(getPoints(keyword), keyword);
            if (pointsKeywords.points.Count != 0)
            {
                if (mOverlays.Count < MAX_CHECKBOXES)
                {
                    makeCheckBox(pointsKeywords.keyword);
                    HeatmapTileProvider provider = new HeatmapTileProvider.Builder().Data(pointsKeywords.points).Gradient(makeGradient(HEATMAP_COLORS[(HeatmapColors)mOverlaysRendered])).Build();
                    TileOverlay overlay = getMap().AddTileOverlay(new TileOverlayOptions().InvokeTileProvider(provider));
                    mOverlays.Add(pointsKeywords.keyword, overlay);
                }
                mOverlaysRendered++;
                if (mOverlaysRendered == mOverlaysInput)
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

        /**
         * Class to store both the points and the keywords, for use in the MakeOverlayTask class.
         */
        private class PointsKeywords
        {
            public List<LatLng> points;
            public string keyword;

            public PointsKeywords(List<LatLng> points, string keyword)
            {
                this.points = points;
                this.keyword = keyword;
            }
        }

        /**
         * Creates a one colored gradient which varies in opacity.
         *
         * @param color The opaque color the gradient should be.
         * @return A gradient made purely of the given color with different alpha values.
         */
        private Gradient makeGradient(int color)
        {
            int[] colors = { color };
            float[] startPoints = { 1.0f };
            return new Gradient(colors, startPoints);
        }

        public bool OnEditorAction(TextView textView, [GeneratedEnum] ImeAction actionId, KeyEvent keyEvent)
        {
            bool handled = false;
            if (actionId == ImeAction.Go)
            {
                submit(null);
                handled = true;
            }
            return handled;
        }

        public void OnClick(View v)
        {
            CheckBox c = (CheckBox)v;
            // Text is the keyword
            TileOverlay overlay = mOverlays.GetValueOrDefault(v.Tag.ToString());
            if (overlay != null)
            {
                overlay.Visible = c.Checked;
            }
        }
    }
}
