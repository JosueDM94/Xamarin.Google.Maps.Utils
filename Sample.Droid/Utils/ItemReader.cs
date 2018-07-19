using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

using Android.Gms.Maps.Model;

using Sample.Droid.Models;

namespace Sample.Droid.Utils
{
    public static class ItemReader
    {
        public static List<ClusterMarker> StreamToClusterMarker(Stream stream)
        {
            var markers = new List<ClusterMarker>();
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            var items = JsonConvert.DeserializeObject<List<Position>>(json);
            foreach(var item in items)
            {
                markers.Add(new ClusterMarker
                {
                    Position = new LatLng(item.Latitude,item.Longitude),
                    Snippet = item.Snippet,
                    Title = item.Title
                });
            }
            return markers;
        }

        public static List<LatLng> StreamToLatLng(Stream stream)
        {
            var list = new List<LatLng>();
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            var items = JsonConvert.DeserializeObject<List<Position>>(json);
            foreach (var item in items)
                list.Add(new LatLng(item.Latitude, item.Longitude));
            return list;
        }
    }
}