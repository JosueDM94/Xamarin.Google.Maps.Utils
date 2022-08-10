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

<p align="center"><img width="90%" vspace="20" src="https://cloud.githubusercontent.com/assets/1950036/6629704/f57bc6d8-c908-11e4-815a-0d909fe02f99.gif"></p>

For more information, check out the detailed guide on the
[Google Developers site][devsite-guide-android]. You can also view the generated
[reference docs][javadoc] for a full list of classes and their methods.

## Google Maps Utils iOS ##

## Usage Overview

- **Marker clustering** — handles the display of a large number of points
- **Quadtree data structure** - indexes 2D geometry points and performs
2D range queries

<p align="center"><img width="90%" vspace="20" src="https://cloud.githubusercontent.com/assets/16808355/16646253/77feeb96-446c-11e6-9ec1-19e12a7fb3ae.png"></p>

- **Geometry libraries** - KML and GeoJSON rendering
<p align="center"><img width="90%" vspace="20" src="https://cloud.githubusercontent.com/assets/16808355/25834988/ca7c3566-34be-11e7-8f07-16c3ae9de63a.png"></p>

- **Heatmaps** - Heatmap rendering
<p align="center"><img width="90%" vspace="20" src="https://user-images.githubusercontent.com/16808355/30678820-54243eb6-9ed8-11e7-81b4-c1afe3df37b3.png"></p>

## Customize cluster and item markers

As of version 1.1.0 we have added new features for easy customization of markers. There is a new delegate [GMUClusterRendererDelegate][gmuclusterrendererdelegate] on ```GMUDefaultClusterRenderer``` which allows developers to customize properties of a marker before and after it is added to the map. Using this new delegate you can achieve something cool like this:

<p align="center"><img vspace="20" src="https://cloud.githubusercontent.com/assets/16808355/18979908/62b15fe2-8712-11e6-9931-cd66fae38cba.png"></p>


See [CustomMarkerViewController][custommarkerviewcontroller] for the
implementation.

## KML and GeoJSON rendering

As of version 2.0.0 we have added new features for rendering KML and GeoJSON inputs. This first
version supports common geometries like Point, Polyline, Polygon, GroundOverlay. Please note that
this version does not support interaction with the rendered geometries.

The following screenshot shows a demo of a KML file being rendered on the map. See
[KMLViewController][kmlviewcontroller] for how to use the new APIs.

<p align="center"><img vspace="20" src="https://cloud.githubusercontent.com/assets/16808355/25834988/ca7c3566-34be-11e7-8f07-16c3ae9de63a.png"></p>

## Heatmap rendering

As of version 2.1.0 we have added new features for rendering heatmaps.
Heatmaps make it easy for viewers to understand the distribution and relative
intensity of data points on a map. Rather than placing a marker at each
location, heatmaps use color to represent the distribution of the data.

In the example below, red represents areas of high concentration of police
stations in Victoria, Australia.

<p align="center">
<img src="https://developers.google.com/maps/documentation/ios-sdk/images/heatmap-ios.png"
     width="250" alt="A map with a heatmap showing location of police stations">
</p>

For more information, check out the detailed guide on the
[Google Developers site][devsite-guide-ios].

[devsite-guide-ios]: https://developers.google.com/maps/documentation/ios-sdk/utility/
[devsite-guide-android]: https://developers.google.com/maps/documentation/android-api/utility/
[javadoc]: http://googlemaps.github.io/android-maps-utils/javadoc/
[android-site]: https://developer.android.com/training/maps/index.html
[ios-site]: https://developers.google.com/maps/documentation/ios-sdk
[kmlviewcontroller]: https://github.com/JosueDM94/Xamarin.Google.Maps.Utils/blob/master/Samples/Sample.iOS/UI/KMLViewController.cs
[custommarkerviewcontroller]: https://github.com/JosueDM94/Xamarin.Google.Maps.Utils/blob/master/Samples/Sample.iOS/UI/CustomMarkerViewController.cs
[gmuclusterrendererdelegate]: https://github.com/googlemaps/google-maps-ios-utils/blob/master/src/Clustering/View/GMUDefaultClusterRenderer.h