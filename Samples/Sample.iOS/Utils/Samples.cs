using System.Collections.Generic;
using Sample.iOS.Models;

namespace Sample.iOS.Utils
{
    public static class Samples
    {
        public static List<Pages> loadSamples()
        {
            return new List<Pages>()
            {
                new Pages("Basic", "Basic Clustering", typeof(BasicViewController)),
                new Pages("Custom Marker", "Custom Clustering", typeof(CustomMarkerViewController)),
                new Pages("KML", "KML Rendering", typeof(KMLViewController)),
                new Pages("GeoJSON", "GeoJSON Rendering", typeof(GeoJSONViewController)),
                new Pages("Heatmaps", "Heatmaps", typeof(HeatmapViewController))
            };
        }
    }
}