using System.Collections.Generic;
using System.IO;
using Java.Util;
using Org.Json;
using Sample.Android.Models;

namespace Sample.Android.Utils
{
    public class MyItemReader
    {
        /*
         * This matches only once in whole input,
         * so Scanner.next returns whole InputStream as a String.
         * http://stackoverflow.com/a/5445161/2183804
         */
        private static string REGEX_INPUT_BOUNDARY_BEGINNING = "\\A";

        public List<MyItem> read(Stream inputStream)
        {
            List<MyItem> items = new List<MyItem>();
            string json = new Scanner(inputStream).UseDelimiter(REGEX_INPUT_BOUNDARY_BEGINNING).Next();
            JSONArray array = new JSONArray(json);
            for (int i = 0; i < array.Length(); i++)
            {
                string title = null;
                string snippet = null;
                JSONObject jsonObject = array.GetJSONObject(i);
                double lat = jsonObject.GetDouble("lat");
                double lng = jsonObject.GetDouble("lng");
                if (!jsonObject.IsNull("title"))
                {
                    title = jsonObject.GetString("title");
                }
                if (!jsonObject.IsNull("snippet"))
                {
                    snippet = jsonObject.GetString("snippet");
                }
                items.Add(new MyItem(lat, lng, title, snippet));
            }
            return items;
        }
    }
}