using UIKit;
using System;
using Foundation;
using Google.Maps;
using ObjCRuntime;
using CoreGraphics;
using CoreLocation;

namespace Google.Maps.Utils
{
    // @protocol GMUClusterItem <NSObject>
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface GMUClusterItem
    {
        // @required @property (readonly, nonatomic) CLLocationCoordinate2D position;
        [Abstract]
        [Export("position")]
        CLLocationCoordinate2D Position { get; }
    }

    interface IGMUClusterItem { }

    // @protocol GMUCluster <NSObject>
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface GMUCluster
    {
        // @required @property (readonly, nonatomic) CLLocationCoordinate2D position;
        [Abstract]
        [Export("position")]
        CLLocationCoordinate2D Position { get; }

        // @required @property (readonly, nonatomic) NSUInteger count;
        [Abstract]
        [Export("count")]
        nuint Count { get; }

        // @required @property (readonly, nonatomic) NSArray<id<GMUClusterItem>> * _Nonnull items;
        [Abstract]
        [Export("items")]
        IGMUClusterItem[] Items { get; }
    }

    interface IGMUCluster{ }

    // @protocol GMUClusterAlgorithm <NSObject>
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface GMUClusterAlgorithm
    {
        // @required -(void)addItems:(NSArray<id<GMUClusterItem>> * _Nonnull)items;
        [Abstract]
        [Export("addItems:")]
        void AddItems(IGMUClusterItem[] items);

        // @required -(void)removeItem:(id<GMUClusterItem> _Nonnull)item;
        [Abstract]
        [Export("removeItem:")]
        void RemoveItem(IGMUClusterItem item);

        // @required -(void)clearItems;
        [Abstract]
        [Export("clearItems")]
        void ClearItems();

        // @required -(NSArray<id<GMUCluster>> * _Nonnull)clustersAtZoom:(float)zoom;
        [Abstract]
        [Export("clustersAtZoom:")]
        IGMUCluster[] ClustersAtZoom(float zoom);
    }

    interface IGMUClusterAlgorithm { }

    // @protocol GMUClusterIconGenerator <NSObject>
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface GMUClusterIconGenerator
    {
        // @required -(UIImage *)iconForSize:(NSUInteger)size;
        [Abstract]
        [Export("iconForSize:")]
        UIImage IconForSize(nuint size);
    }

    interface IGMUClusterIconGenerator{ }

    // @protocol GMUClusterRenderer <NSObject>
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface GMUClusterRenderer
    {
        // @required -(void)renderClusters:(NSArray<id<GMUCluster>> * _Nonnull)clusters;
        [Abstract]
        [Export("renderClusters:")]
        void RenderClusters(IGMUCluster[] clusters);

        // @required -(void)update;
        [Abstract]
        [Export("update")]
        void Update();
    }

    interface IGMUClusterRenderer { }

    // @protocol GMUClusterManagerDelegate <NSObject>
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface GMUClusterManagerDelegate
    {
        // @optional -(BOOL)clusterManager:(GMUClusterManager * _Nonnull)clusterManager didTapCluster:(id<GMUCluster> _Nonnull)cluster;
        [Export("clusterManager:didTapCluster:")]
        bool DidTapCluster(GMUClusterManager clusterManager, IGMUCluster cluster);

        // @optional -(BOOL)clusterManager:(GMUClusterManager * _Nonnull)clusterManager didTapClusterItem:(id<GMUClusterItem> _Nonnull)clusterItem;
        [Export("clusterManager:didTapClusterItem:")]
        bool DidTapClusterItem(GMUClusterManager clusterManager, IGMUClusterItem clusterItem);
    }

    interface IGMUClusterManagerDelegate { }

    // @interface GMUClusterManager : NSObject <GMSMapViewDelegate>
    [BaseType(typeof(NSObject))]
    interface GMUClusterManager : IMapViewDelegate
    {
        // -(instancetype _Nonnull)initWithMap:(GMSMapView * _Nonnull)mapView algorithm:(id<GMUClusterAlgorithm> _Nonnull)algorithm renderer:(id<GMUClusterRenderer> _Nonnull)renderer __attribute__((objc_designated_initializer));
        [Export("initWithMap:algorithm:renderer:")]
        [DesignatedInitializer]
        IntPtr Constructor(MapView mapView, IGMUClusterAlgorithm algorithm, IGMUClusterRenderer renderer);

        // @property (readonly, nonatomic) id<GMUClusterAlgorithm> _Nonnull algorithm;
        [Export("algorithm")]
        IGMUClusterAlgorithm Algorithm { get; }

        [Wrap("WeakDelegate")]
        [NullAllowed]
        IGMUClusterManagerDelegate Delegate { get; }

        // @property (readonly, nonatomic, weak) id<GMUClusterManagerDelegate> _Nullable delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        IGMUClusterManagerDelegate WeakDelegate { get; }

        [Wrap("WeakMapDelegate")]
        [NullAllowed]
        IMapViewDelegate MapDelegate { get; }

        // @property (readonly, nonatomic, weak) id<GMSMapViewDelegate> _Nullable mapDelegate;
        [NullAllowed, Export("mapDelegate", ArgumentSemantic.Weak)]
        IMapViewDelegate WeakMapDelegate { get; }

        // -(void)setDelegate:(id<GMUClusterManagerDelegate> _Nullable)delegate mapDelegate:(id<GMSMapViewDelegate> _Nullable)mapDelegate;
        [Export("setDelegate:mapDelegate:")]
        void SetDelegate([NullAllowed] IGMUClusterManagerDelegate @delegate, [NullAllowed] IMapViewDelegate mapDelegate);

        // -(void)addItem:(id<GMUClusterItem> _Nonnull)item;
        [Export("addItem:")]
        void AddItem(IGMUClusterItem item);

        // -(void)addItems:(NSArray<id<GMUClusterItem>> * _Nonnull)items;
        [Export("addItems:")]
        void AddItems(IGMUClusterItem[] items);

        // -(void)removeItem:(id<GMUClusterItem> _Nonnull)item;
        [Export("removeItem:")]
        void RemoveItem(IGMUClusterItem item);

        // -(void)clearItems;
        [Export("clearItems")]
        void ClearItems();

        // -(void)cluster;
        [Export("cluster")]
        void Cluster();
    }

    // @interface GMUDefaultClusterIconGenerator : NSObject <GMUClusterIconGenerator>
    [BaseType(typeof(NSObject))]
    interface GMUDefaultClusterIconGenerator : GMUClusterIconGenerator
    {
        // -(instancetype _Nonnull)initWithBuckets:(NSArray<NSNumber *> * _Nonnull)buckets;
        [Export("initWithBuckets:")]
        IntPtr Constructor(NSNumber[] buckets);

        // -(instancetype _Nonnull)initWithBuckets:(NSArray<NSNumber *> * _Nonnull)buckets backgroundImages:(NSArray<UIImage *> * _Nonnull)backgroundImages;
        [Export("initWithBuckets:backgroundImages:")]
        IntPtr Constructor(NSNumber[] buckets, UIImage[] backgroundImages);

        // -(instancetype _Nonnull)initWithBuckets:(NSArray<NSNumber *> * _Nonnull)buckets backgroundColors:(NSArray<UIColor *> * _Nonnull)backgroundColors;
        [Export("initWithBuckets:backgroundColors:")]
        IntPtr Constructor(NSNumber[] buckets, UIColor[] backgroundColors);

        // -(UIImage * _Nonnull)iconForSize:(NSUInteger)size;
        [Export("iconForSize:")]
        UIImage IconForSize(nuint size);
    }

    // @protocol GMUClusterRendererDelegate <NSObject>
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface GMUClusterRendererDelegate
    {
        // @optional -(GMSMarker * _Nullable)renderer:(id<GMUClusterRenderer> _Nonnull)renderer markerForObject:(id _Nonnull)object;
        [Export("renderer:markerForObject:")]
        [return: NullAllowed]
        Marker MarkerForObject(IGMUClusterRenderer renderer, NSObject @object);

        // @optional -(void)renderer:(id<GMUClusterRenderer> _Nonnull)renderer willRenderMarker:(GMSMarker * _Nonnull)marker;
        [Export("renderer:willRenderMarker:")]
        void WillRenderMarker(IGMUClusterRenderer renderer, Marker marker);

        // @optional -(void)renderer:(id<GMUClusterRenderer> _Nonnull)renderer didRenderMarker:(GMSMarker * _Nonnull)marker;
        [Export("renderer:didRenderMarker:")]
        void DidRenderMarker(IGMUClusterRenderer renderer, Marker marker);
    }

    interface IGMUClusterRendererDelegate { }

    // @interface GMUDefaultClusterRenderer : NSObject <GMUClusterRenderer>
    [BaseType(typeof(NSObject))]
    interface GMUDefaultClusterRenderer : GMUClusterRenderer
    {
        // @property (nonatomic) BOOL animatesClusters;
        [Export("animatesClusters")]
        bool AnimatesClusters { get; set; }

        // @property (nonatomic) int zIndex;
        [Export("zIndex")]
        int ZIndex { get; set; }

        [Wrap("WeakDelegate")]
        [NullAllowed]
        IGMUClusterRendererDelegate Delegate { get; set; }

        // @property (nonatomic, weak) id<GMUClusterRendererDelegate> _Nullable delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        IGMUClusterRendererDelegate WeakDelegate { get; set; }

        // -(instancetype _Nonnull)initWithMapView:(GMSMapView * _Nonnull)mapView clusterIconGenerator:(id<GMUClusterIconGenerator> _Nonnull)iconGenerator;
        [Export("initWithMapView:clusterIconGenerator:")]
        IntPtr Constructor(MapView mapView, IGMUClusterIconGenerator iconGenerator);

        // -(BOOL)shouldRenderAsCluster:(id<GMUCluster> _Nonnull)cluster atZoom:(float)zoom;
        [Export("shouldRenderAsCluster:atZoom:")]
        bool ShouldRenderAsCluster(IGMUCluster cluster, float zoom);
    }

    // @protocol GMUGeometry <NSObject>
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface GMUGeometry
    {
        // @required @property (readonly, nonatomic) NSString * _Nonnull type;
        [Abstract]
        [Export("type")]
        string Type { get; }
    }

    interface IGMUGeometry { }

    // @interface GMUStyle : NSObject
    [BaseType(typeof(NSObject))]
    interface GMUStyle
    {
        // @property (readonly, nonatomic) NSString * _Nonnull styleID;
        [Export("styleID")]
        string StyleID { get; }

        // @property (readonly, nonatomic) UIColor * _Nullable strokeColor;
        [NullAllowed, Export("strokeColor")]
        UIColor StrokeColor { get; }

        // @property (readonly, nonatomic) UIColor * _Nullable fillColor;
        [NullAllowed, Export("fillColor")]
        UIColor FillColor { get; }

        // @property (readonly, nonatomic) CGFloat width;
        [Export("width")]
        nfloat Width { get; }

        // @property (readonly, nonatomic) CGFloat scale;
        [Export("scale")]
        nfloat Scale { get; }

        // @property (readonly, nonatomic) CGFloat heading;
        [Export("heading")]
        nfloat Heading { get; }

        // @property (readonly, nonatomic) CGPoint anchor;
        [Export("anchor")]
        CGPoint Anchor { get; }

        // @property (readonly, nonatomic) NSString * _Nullable iconUrl;
        [NullAllowed, Export("iconUrl")]
        string IconUrl { get; }

        // @property (readonly, nonatomic) NSString * _Nullable title;
        [NullAllowed, Export("title")]
        string Title { get; }

        // @property (readonly, nonatomic) BOOL hasFill;
        [Export("hasFill")]
        bool HasFill { get; }

        // @property (readonly, nonatomic) BOOL hasStroke;
        [Export("hasStroke")]
        bool HasStroke { get; }

        // -(instancetype _Nonnull)initWithStyleID:(NSString * _Nonnull)styleID strokeColor:(UIColor * _Nullable)strokeColor fillColor:(UIColor * _Nullable)fillColor width:(CGFloat)width scale:(CGFloat)scale heading:(CGFloat)heading anchor:(CGPoint)anchor iconUrl:(NSString * _Nullable)iconUrl title:(NSString * _Nullable)title hasFill:(BOOL)hasFill hasStroke:(BOOL)hasStroke;
        [Export("initWithStyleID:strokeColor:fillColor:width:scale:heading:anchor:iconUrl:title:hasFill:hasStroke:")]
        IntPtr Constructor(string styleID, [NullAllowed] UIColor strokeColor, [NullAllowed] UIColor fillColor, nfloat width, nfloat scale, nfloat heading, CGPoint anchor, [NullAllowed] string iconUrl, [NullAllowed] string title, bool hasFill, bool hasStroke);
    }

    // @protocol GMUGeometryContainer <NSObject>
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface GMUGeometryContainer
    {
        // @required @property (readonly, nonatomic) id<GMUGeometry> _Nonnull geometry;
        [Abstract]
        [Export("geometry")]
        IGMUGeometry Geometry { get; }

        // @required @property (nonatomic) GMUStyle * _Nullable style;
        [Abstract]
        [NullAllowed, Export("style", ArgumentSemantic.Assign)]
        GMUStyle Style { get; set; }
    }

    interface IGMUGeometryContainer { }

    // @interface GMUFeature : NSObject <GMUGeometryContainer>
    [BaseType(typeof(NSObject))]
    interface GMUFeature : IGMUGeometryContainer
    {
        // @property (readonly, nonatomic) NSString * _Nullable identifier;
        [NullAllowed, Export("identifier")]
        string Identifier { get; }

        // @property (readonly, nonatomic) NSDictionary<NSString *,NSString *> * _Nullable properties;
        [NullAllowed, Export("properties")]
        NSDictionary<NSString, NSString> Properties { get; }

        // @property (readonly, nonatomic) GMSCoordinateBounds * _Nullable boundingBox;
        [NullAllowed, Export("boundingBox")]
        CoordinateBounds BoundingBox { get; }

        // -(instancetype _Nonnull)initWithGeometry:(id<GMUGeometry> _Nonnull)geometry identifier:(NSString * _Nullable)identifier properties:(NSDictionary<NSString *,NSString *> * _Nullable)properties boundingBox:(GMSCoordinateBounds * _Nullable)boundingBox;
        [Export("initWithGeometry:identifier:properties:boundingBox:")]
        IntPtr Constructor(IGMUGeometry geometry, [NullAllowed] string identifier, [NullAllowed] NSDictionary<NSString, NSString> properties, [NullAllowed] CoordinateBounds boundingBox);
    }

    // @interface GMUGeoJSONParser : NSObject
    [BaseType(typeof(NSObject))]
    interface GMUGeoJSONParser
    {
        // @property (readonly, nonatomic) NSArray<id<GMUGeometryContainer>> * _Nonnull features;
        [Export("features")]
        IGMUGeometryContainer[] Features { get; }

        // -(instancetype _Nonnull)initWithURL:(NSURL * _Nonnull)url;
        [Export("initWithURL:")]
        IntPtr Constructor(NSUrl url);

        // -(instancetype _Nonnull)initWithData:(NSData * _Nonnull)data;
        [Export("initWithData:")]
        IntPtr Constructor(NSData data);

        // -(instancetype _Nonnull)initWithStream:(NSInputStream * _Nonnull)stream;
        [Export("initWithStream:")]
        IntPtr Constructor(NSInputStream stream);

        // -(void)parse;
        [Export("parse")]
        void Parse();
    }

    // @interface GMUGeometryCollection : NSObject <GMUGeometry>
    [BaseType(typeof(NSObject))]
    interface GMUGeometryCollection : IGMUGeometry
    {
        // @property (readonly, nonatomic) NSArray<id<GMUGeometry>> * _Nonnull geometries;
        [Export("geometries")]
        IGMUGeometry[] Geometries { get; }

        // -(instancetype _Nonnull)initWithGeometries:(NSArray<id<GMUGeometry>> * _Nonnull)geometries;
        [Export("initWithGeometries:")]
        IntPtr Constructor(IGMUGeometry[] geometries);
    }

    // @interface GMUGeometryRenderer : NSObject
    [BaseType(typeof(NSObject))]
    interface GMUGeometryRenderer
    {
        // -(instancetype _Nonnull)initWithMap:(GMSMapView * _Nonnull)map geometries:(NSArray<id<GMUGeometryContainer>> * _Nonnull)geometries;
        [Export("initWithMap:geometries:")]
        IntPtr Constructor(MapView map, IGMUGeometryContainer[] geometries);

        // -(instancetype _Nonnull)initWithMap:(GMSMapView * _Nonnull)map geometries:(NSArray<id<GMUGeometryContainer>> * _Nonnull)geometries styles:(NSArray<GMUStyle *> * _Nullable)styles;
        [Export("initWithMap:geometries:styles:")]
        IntPtr Constructor(MapView map, IGMUGeometryContainer[] geometries, [NullAllowed] GMUStyle[] styles);

        // -(void)render;
        [Export("render")]
        void Render();

        // -(void)clear;
        [Export("clear")]
        void Clear();
    }

    // @interface GMUGradient : NSObject
    [BaseType(typeof(NSObject))]
    interface GMUGradient
    {
        // @property (readonly, nonatomic) NSUInteger mapSize;
        [Export("mapSize")]
        nuint MapSize { get; }

        // @property (readonly, nonatomic) NSArray<UIColor *> * _Nonnull colors;
        [Export("colors")]
        UIColor[] Colors { get; }

        // @property (readonly, nonatomic) NSArray<NSNumber *> * _Nonnull startPoints;
        [Export("startPoints")]
        NSNumber[] StartPoints { get; }

        // -(instancetype _Nonnull)initWithColors:(NSArray<UIColor *> * _Nonnull)colors startPoints:(NSArray<NSNumber *> * _Nonnull)startPoints colorMapSize:(NSUInteger)mapSize;
        [Export("initWithColors:startPoints:colorMapSize:")]
        IntPtr Constructor(UIColor[] colors, NSNumber[] startPoints, nuint mapSize);

        // -(NSArray<UIColor *> * _Nonnull)generateColorMap;
        [Export("generateColorMap")]
        UIColor[] GenerateColorMap { get; }
    }

    // @interface GMUGridBasedClusterAlgorithm : NSObject <GMUClusterAlgorithm>
    [BaseType(typeof(NSObject))]
    interface GMUGridBasedClusterAlgorithm : GMUClusterAlgorithm
    {
    }

    // @interface GMUGroundOverlay : NSObject <GMUGeometry>
    [BaseType(typeof(NSObject))]
    interface GMUGroundOverlay : IGMUGeometry
    {
        // @property (readonly, nonatomic) CLLocationCoordinate2D northEast;
        [Export("northEast")]
        CLLocationCoordinate2D NorthEast { get; }

        // @property (readonly, nonatomic) CLLocationCoordinate2D southWest;
        [Export("southWest")]
        CLLocationCoordinate2D SouthWest { get; }

        // @property (readonly, nonatomic) int zIndex;
        [Export("zIndex")]
        int ZIndex { get; }

        // @property (readonly, nonatomic) double rotation;
        [Export("rotation")]
        double Rotation { get; }

        // @property (readonly, nonatomic) NSString * _Nonnull href;
        [Export("href")]
        string Href { get; }

        // -(instancetype _Nonnull)initWithCoordinate:(CLLocationCoordinate2D)northEast southWest:(CLLocationCoordinate2D)southWest zIndex:(int)zIndex rotation:(double)rotation href:(NSString * _Nonnull)href;
        [Export("initWithCoordinate:southWest:zIndex:rotation:href:")]
        IntPtr Constructor(CLLocationCoordinate2D northEast, CLLocationCoordinate2D southWest, int zIndex, double rotation, string href);
    }

    // @protocol GQTPointQuadTreeItem <NSObject>
    [Protocol,Model]
    [BaseType(typeof(NSObject))]
    interface GQTPointQuadTreeItem
    {
        // @required -(GQTPoint)point;
        [Abstract]
        [Export("point")]
        GQTPoint Point { get; }
    }

    interface IGQTPointQuadTreeItem { }

    // @interface GMUWeightedLatLng : NSObject <GQTPointQuadTreeItem>
    [BaseType(typeof(NSObject))]
    interface GMUWeightedLatLng : IGQTPointQuadTreeItem
    {
        // @property (readonly, nonatomic) float intensity;
        [Export("intensity")]
        float Intensity { get; }

        // -(instancetype _Nonnull)initWithCoordinate:(CLLocationCoordinate2D)coordinate intensity:(float)intensity;
        [Export("initWithCoordinate:intensity:")]
        IntPtr Constructor(CLLocationCoordinate2D coordinate, float intensity);
    }

    // @interface GMUHeatmapTileLayer : GMSSyncTileLayer
    [BaseType(typeof(SyncTileLayer))]
    interface GMUHeatmapTileLayer
    {
        // @property (copy, nonatomic) NSArray<GMUWeightedLatLng *> * _Nonnull weightedData;
        [Export("weightedData", ArgumentSemantic.Copy)]
        GMUWeightedLatLng[] WeightedData { get; set; }

        // @property (nonatomic) NSUInteger radius;
        [Export("radius")]
        nuint Radius { get; set; }

        // @property (nonatomic) GMUGradient * _Nonnull gradient;
        [Export("gradient", ArgumentSemantic.Assign)]
        GMUGradient Gradient { get; set; }

        // @property (nonatomic) NSUInteger minimumZoomIntensity;
        [Export("minimumZoomIntensity")]
        nuint MinimumZoomIntensity { get; set; }

        // @property (nonatomic) NSUInteger maximumZoomIntensity;
        [Export("maximumZoomIntensity")]
        nuint MaximumZoomIntensity { get; set; }
    }

    // @interface GMUKMLParser : NSObject
    [BaseType(typeof(NSObject))]
    interface GMUKMLParser
    {
        // @property (readonly, nonatomic) NSArray<id<GMUGeometryContainer>> * _Nonnull placemarks;
        [Export("placemarks")]
        IGMUGeometryContainer[] Placemarks { get; }

        // @property (readonly, nonatomic) NSArray<GMUStyle *> * _Nonnull styles;
        [Export("styles")]
        GMUStyle[] Styles { get; }

        // -(void)parse;
        [Export("parse")]
        void Parse();

        // -(instancetype _Nonnull)initWithURL:(NSURL * _Nonnull)url;
        [Export("initWithURL:")]
        IntPtr Constructor(NSUrl url);

        // -(instancetype _Nonnull)initWithData:(NSData * _Nonnull)data;
        [Export("initWithData:")]
        IntPtr Constructor(NSData data);

        // -(instancetype _Nonnull)initWithStream:(NSInputStream * _Nonnull)stream;
        [Export("initWithStream:")]
        IntPtr Constructor(NSInputStream stream);
    }

    // @interface GMULineString : NSObject <GMUGeometry>
    [BaseType(typeof(NSObject))]
    interface GMULineString : IGMUGeometry
    {
        // @property (readonly, nonatomic) GMSPath * _Nonnull path;
        [Export("path")]
        Path Path { get; }

        // -(instancetype _Nonnull)initWithPath:(GMSPath * _Nonnull)path;
        [Export("initWithPath:")]
        IntPtr Constructor(Path path);
    }

    // @interface GMUNonHierarchicalDistanceBasedAlgorithm : NSObject <GMUClusterAlgorithm>
    [BaseType(typeof(NSObject))]
    interface GMUNonHierarchicalDistanceBasedAlgorithm : GMUClusterAlgorithm
    {
    }

    // @interface GMUPlacemark : NSObject <GMUGeometryContainer>
    [BaseType(typeof(NSObject))]
    interface GMUPlacemark : IGMUGeometryContainer
    {
        // @property (readonly, nonatomic) NSString * _Nullable title;
        [NullAllowed, Export("title")]
        string Title { get; }

        // @property (readonly, nonatomic) NSString * _Nullable snippet;
        [NullAllowed, Export("snippet")]
        string Snippet { get; }

        // @property (readonly, nonatomic) NSString * _Nullable styleUrl;
        [NullAllowed, Export("styleUrl")]
        string StyleUrl { get; }

        // -(instancetype _Nonnull)initWithGeometry:(id<GMUGeometry> _Nullable)geometry title:(NSString * _Nullable)title snippet:(NSString * _Nullable)snippet style:(GMUStyle * _Nullable)style styleUrl:(NSString * _Nullable)styleUrl;
        [Export("initWithGeometry:title:snippet:style:styleUrl:")]
        IntPtr Constructor([NullAllowed] IGMUGeometry geometry, [NullAllowed] string title, [NullAllowed] string snippet, [NullAllowed] GMUStyle style, [NullAllowed] string styleUrl);
    }

    // @interface GMUPoint : NSObject <GMUGeometry>
    [BaseType(typeof(NSObject))]
    interface GMUPoint : IGMUGeometry
    {
        // @property (readonly, nonatomic) CLLocationCoordinate2D coordinate;
        [Export("coordinate")]
        CLLocationCoordinate2D Coordinate { get; }

        // -(instancetype _Nonnull)initWithCoordinate:(CLLocationCoordinate2D)coordinate;
        [Export("initWithCoordinate:")]
        IntPtr Constructor(CLLocationCoordinate2D coordinate);
    }

    // @interface GMUPolygon : NSObject <GMUGeometry>
    [BaseType(typeof(NSObject))]
    interface GMUPolygon : IGMUGeometry
    {
        // @property (readonly, nonatomic) NSArray<GMSPath *> * _Nonnull paths;
        [Export("paths")]
        Path[] Paths { get; }

        // -(instancetype _Nonnull)initWithPaths:(NSArray<GMSPath *> * _Nonnull)paths;
        [Export("initWithPaths:")]
        IntPtr Constructor(Path[] paths);
    }

    // @interface GMUSimpleClusterAlgorithm : NSObject <GMUClusterAlgorithm>
    [BaseType(typeof(NSObject))]
    interface GMUSimpleClusterAlgorithm : GMUClusterAlgorithm
    {
    }

    // @interface GMUStaticCluster : NSObject <GMUCluster>
    [BaseType(typeof(NSObject))]
    interface GMUStaticCluster : IGMUCluster
    {
        // -(instancetype _Nonnull)initWithPosition:(CLLocationCoordinate2D)position __attribute__((objc_designated_initializer));
        [Export("initWithPosition:")]
        [DesignatedInitializer]
        IntPtr Constructor(CLLocationCoordinate2D position);

        // @property (readonly, nonatomic) CLLocationCoordinate2D position;
        [Export("position")]
        CLLocationCoordinate2D Position { get; }

        // @property (readonly, nonatomic) NSUInteger count;
        [Export("count")]
        nuint Count { get; }

        // @property (readonly, nonatomic) NSArray<id<GMUClusterItem>> * _Nonnull items;
        [Export("items")]
        IGMUClusterItem[] Items { get; }

        // -(void)addItem:(id<GMUClusterItem> _Nonnull)item;
        [Export("addItem:")]
        void AddItem(IGMUClusterItem item);

        // -(void)removeItem:(id<GMUClusterItem> _Nonnull)item;
        [Export("removeItem:")]
        void RemoveItem(IGMUClusterItem item);
    }

    // @interface GMUWrappingDictionaryKey : NSObject <NSCopying>
    [BaseType(typeof(NSObject))]
    interface GMUWrappingDictionaryKey : INSCopying
    {
        // -(instancetype)initWithObject:(id)object;
        [Export("initWithObject:")]
        IntPtr Constructor(NSObject @object);
    }

    // @interface GQTPointQuadTree : NSObject
    [BaseType(typeof(NSObject))]
    interface GQTPointQuadTree
    {
        // -(id)initWithBounds:(GQTBounds)bounds;
        [Export("initWithBounds:")]
        IntPtr Constructor(GQTBounds bounds);

        // -(BOOL)add:(id<GQTPointQuadTreeItem>)item;
        [Export("add:")]
        bool Add(IGQTPointQuadTreeItem item);

        // -(BOOL)remove:(id<GQTPointQuadTreeItem>)item;
        [Export("remove:")]
        bool Remove(IGQTPointQuadTreeItem item);

        // -(void)clear;
        [Export("clear")]
        void Clear();

        // -(NSArray *)searchWithBounds:(GQTBounds)bounds;
        [Export("searchWithBounds:")]
        NSObject[] SearchWithBounds(GQTBounds bounds);

        // -(NSUInteger)count;
        [Export("count")]
        nuint Count { get; }
    }

    // @interface GQTPointQuadTreeChild : NSObject
    [BaseType(typeof(NSObject))]
    interface GQTPointQuadTreeChild
    {
        // -(void)add:(id<GQTPointQuadTreeItem>)item withOwnBounds:(GQTBounds)bounds atDepth:(NSUInteger)depth;
        [Export("add:withOwnBounds:atDepth:")]
        void Add(IGQTPointQuadTreeItem item, GQTBounds bounds, nuint depth);

        // -(BOOL)remove:(id<GQTPointQuadTreeItem>)item withOwnBounds:(GQTBounds)bounds;
        [Export("remove:withOwnBounds:")]
        bool Remove(IGQTPointQuadTreeItem item, GQTBounds bounds);

        // -(void)searchWithBounds:(GQTBounds)searchBounds withOwnBounds:(GQTBounds)ownBounds results:(NSMutableArray *)accumulator;
        [Export("searchWithBounds:withOwnBounds:results:")]
        void SearchWithBounds(GQTBounds searchBounds, GQTBounds ownBounds, NSMutableArray accumulator);

        // -(void)splitWithOwnBounds:(GQTBounds)ownBounds atDepth:(NSUInteger)depth;
        [Export("splitWithOwnBounds:atDepth:")]
        void SplitWithOwnBounds(GQTBounds ownBounds, nuint depth);
    }
}
