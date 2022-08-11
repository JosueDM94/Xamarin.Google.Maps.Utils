# Xamarin.Google.Maps.Utils

[![NuGet Badge](https://buildstats.info/nuget/Xamarin.Google.Maps.Utils)](https://www.nuget.org/packages/Xamarin.Google.Maps.Utils/)

This is a set of Xamarin bindings of Google Maps Utils for iOS and Android

This open-source library contains utilities that are useful for a wide range of applications using the [Google Maps Android API][android-site] and [Google Maps SDK for iOS][ios-site].

## Table of Contents ##

- [Installation](#installation)
- [Example](#example)
- [Android](#google-maps-utils-android)
- [iOS](#google-maps-utils-ios)

## Installation ##

The latest stable release of the AffirmSDK is [available on NuGet](https://www.nuget.org/packages/Xamarin.Google.Maps.Utils).

## Example ##

A demo app that integrates Affirm is included in the repo. You may clone the [GitHub repository](https://github.com/JosueDM94/Xamarin.Google.Maps.Utils) into a new Visual Studio project folder and run the Examples project.

## Google Maps Utils Android ##

## Usage Overview

- **Marker clustering** — handles the display of a large number of points
- **Heat maps** — display a large number of points as a heat map
- **IconGenerator** — display text on your Markers
- **Poly decoding and encoding** — compact encoding for paths,
  interoperability with Maps API web services
- **Spherical geometry** — for example: computeDistance, computeHeading,
  computeArea
- **KML** — displays KML data
- **GeoJSON** — displays and styles GeoJSON data

![Android Usage](https://raw.githubusercontent.com/JosueDM94/Xamarin.Google.Maps.Utils/master/Assets/f57bc6d8-c908-11e4-815a-0d909fe02f99.gif)

For more information, check out the detailed guide on the [Google Developers site][devsite-guide-android]. You can also view the generated [reference docs][javadoc] for a full list of classes and their methods.

## Google Maps Utils iOS ##

## Usage Overview

- **Marker clustering** — handles the display of a large number of points
- **Quadtree data structure** - indexes 2D geometry points and performs
2D range queries

![Quadtree data structure](https://raw.githubusercontent.com/JosueDM94/Xamarin.Google.Maps.Utils/master/Assets/77feeb96-446c-11e6-9ec1-19e12a7fb3ae.png)

- **Geometry libraries** - KML and GeoJSON rendering

![Geometry libraries](https://raw.githubusercontent.com/JosueDM94/Xamarin.Google.Maps.Utils/master/Assets/ca7c3566-34be-11e7-8f07-16c3ae9de63a.png)

- **Heatmaps** - Heatmap rendering

![Heatmaps](https://raw.githubusercontent.com/JosueDM94/Xamarin.Google.Maps.Utils/master/Assets/30678820-54243eb6-9ed8-11e7-81b4-c1afe3df37b3.png)

## Customize cluster and item markers

As of version 1.1.0 we have added new features for easy customization of markers. There is a new delegate [GMUClusterRendererDelegate][gmuclusterrendererdelegate] on ```GMUDefaultClusterRenderer``` which allows developers to customize properties of a marker before and after it is added to the map. Using this new delegate you can achieve something cool like this:

![Customize cluster and item markers](https://raw.githubusercontent.com/JosueDM94/Xamarin.Google.Maps.Utils/master/Assets/62b15fe2-8712-11e6-9931-cd66fae38cba.png)


See [CustomMarkerViewController][custommarkerviewcontroller] for the implementation.

## KML and GeoJSON rendering

As of version 2.0.0 we have added new features for rendering KML and GeoJSON inputs. This first version supports common geometries like Point, Polyline, Polygon, GroundOverlay. Please note that this version does not support interaction with the rendered geometries.

The following screenshot shows a demo of a KML file being rendered on the map. See [KMLViewController][kmlviewcontroller] for how to use the new APIs.

![KML and GeoJSON rendering](https://raw.githubusercontent.com/JosueDM94/Xamarin.Google.Maps.Utils/master/Assets/ca7c3566-34be-11e7-8f07-16c3ae9de63a.png)

## Heatmap rendering

As of version 2.1.0 we have added new features for rendering heatmaps.
Heatmaps make it easy for viewers to understand the distribution and relative intensity of data points on a map. Rather than placing a marker at each location, heatmaps use color to represent the distribution of the data.

In the example below, red represents areas of high concentration of police stations in Victoria, Australia.

![Heatmap rendering](https://raw.githubusercontent.com/JosueDM94/Xamarin.Google.Maps.Utils/master/Assets/heatmap-ios.png)

*A map with a heatmap showing location of police stations*

For more information, check out the detailed guide on the [Google Developers site][devsite-guide-ios].

[devsite-guide-ios]: https://developers.google.com/maps/documentation/ios-sdk/utility/
[devsite-guide-android]: https://developers.google.com/maps/documentation/android-api/utility/
[javadoc]: http://googlemaps.github.io/android-maps-utils/javadoc/
[android-site]: https://developer.android.com/training/maps/index.html
[ios-site]: https://developers.google.com/maps/documentation/ios-sdk
[kmlviewcontroller]: https://github.com/JosueDM94/Xamarin.Google.Maps.Utils/blob/master/Samples/Sample.iOS/UI/KMLViewController.cs
[custommarkerviewcontroller]: https://github.com/JosueDM94/Xamarin.Google.Maps.Utils/blob/master/Samples/Sample.iOS/UI/CustomMarkerViewController.cs
[gmuclusterrendererdelegate]: https://github.com/googlemaps/google-maps-ios-utils/blob/master/src/Clustering/View/GMUDefaultClusterRenderer.h