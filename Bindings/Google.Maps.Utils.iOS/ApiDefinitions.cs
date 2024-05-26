using CoreGraphics;
using CoreLocation;
using Foundation;
using ObjCRuntime;
using UIKit;
using System.Runtime.InteropServices;
using GMSCoordinateBounds = Google.Maps.CoordinateBounds;
using GMSMapView = Google.Maps.MapView;
using GMSMarker = Google.Maps.Marker;
using GMSPath = Google.Maps.Path;
using GMSSyncTileLayer = Google.Maps.SyncTileLayer;
using IGMSMapViewDelegate = Google.Maps.IMapViewDelegate;

namespace Google.Maps.Utils
{
    // @protocol GMUClusterItem <NSObject>
    /*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
    [Protocol]
    [BaseType(typeof(NSObject))]
    interface GMUClusterItem
    {
        // @required @property (readonly, nonatomic) CLLocationCoordinate2D position;
        [Abstract]
        [Export("position")]
        CLLocationCoordinate2D Position { get; }

        // @optional @property (copy, nonatomic) NSString * _Nullable title;
        [NullAllowed, Export("title")]
        string Title { get; set; }

        // @optional @property (copy, nonatomic) NSString * _Nullable snippet;
        [NullAllowed, Export("snippet")]
        string Snippet { get; set; }
    }

    interface IGMUClusterItem { }

    // @interface GMSMarker_GMUClusteritem (GMSMarker) <GMUClusterItem>
    //[Category]
    //[BaseType(typeof(GMSMarker))]
    //interface GMSMarker_GMSMarker_GMUClusteritem : GMUClusterItem
    //{
    //}

    // @protocol GMUCluster <NSObject>
    /*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
    [Protocol]
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

    interface IGMUCluster { }

    // @protocol GMUClusterAlgorithm <NSObject>
    /*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
    [Protocol]
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
    /*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
    [Protocol]
    [BaseType(typeof(NSObject))]
    interface GMUClusterIconGenerator
    {
        // @required -(UIImage *)iconForSize:(NSUInteger)size;
        [Abstract]
        [Export("iconForSize:")]
        UIImage IconForSize(nuint size);
    }

    interface IGMUClusterIconGenerator { }

    // @protocol GMUClusterRenderer <NSObject>
    /*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
    [Protocol]
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
    [Protocol, Model]
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
    [DisableDefaultCtor]
    interface GMUClusterManager : IGMSMapViewDelegate
    {
        // -(instancetype _Nonnull)initWithMap:(GMSMapView * _Nonnull)mapView algorithm:(id<GMUClusterAlgorithm> _Nonnull)algorithm renderer:(id<GMUClusterRenderer> _Nonnull)renderer __attribute__((objc_designated_initializer));
        [Export("initWithMap:algorithm:renderer:")]
        [DesignatedInitializer]
        NativeHandle Constructor(GMSMapView mapView, IGMUClusterAlgorithm algorithm, IGMUClusterRenderer renderer);

        // @property (readonly, nonatomic) id<GMUClusterAlgorithm> _Nonnull algorithm;
        [Export("algorithm")]
        IGMUClusterAlgorithm Algorithm { get; }

        [Wrap("WeakDelegate")]
        [NullAllowed]
        IGMUClusterManagerDelegate Delegate { get; }

        // @property (readonly, nonatomic, weak) id<GMUClusterManagerDelegate> _Nullable delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; }

        [Wrap("WeakMapDelegate")]
        [NullAllowed]
        IGMSMapViewDelegate MapDelegate { get; }

        // @property (readonly, nonatomic, weak) id<GMSMapViewDelegate> _Nullable mapDelegate;
        [NullAllowed, Export("mapDelegate", ArgumentSemantic.Weak)]
        NSObject WeakMapDelegate { get; }

        // -(void)setMapDelegate:(id<GMSMapViewDelegate> _Nullable)mapDelegate;
        [Export("setMapDelegate:")]
        void SetMapDelegate([NullAllowed] IGMSMapViewDelegate mapDelegate);

        // -(void)setDelegate:(id<GMUClusterManagerDelegate> _Nullable)delegate mapDelegate:(id<GMSMapViewDelegate> _Nullable)mapDelegate;
        [Export("setDelegate:mapDelegate:")]
        void SetDelegate([NullAllowed] IGMUClusterManagerDelegate @delegate, [NullAllowed] IGMSMapViewDelegate mapDelegate);

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

    // @interface Testing (GMUClusterManager)
    [Category]
    [BaseType(typeof(GMUClusterManager))]
    interface GMUClusterManagerTesting
    {
        // -(NSUInteger)clusterRequestCount;
        [Export("clusterRequestCount")]
        //[Verify(MethodToProperty)]
        nuint ClusterRequestCount();
    }

    // @interface GMUDefaultClusterIconGenerator : NSObject <GMUClusterIconGenerator>
    [BaseType(typeof(NSObject))]
    interface GMUDefaultClusterIconGenerator : GMUClusterIconGenerator
    {
        // -(instancetype _Nonnull)initWithBuckets:(NSArray<NSNumber *> * _Nonnull)buckets;
        [Export("initWithBuckets:")]
        NativeHandle Constructor(NSNumber[] buckets);

        // -(instancetype _Nonnull)initWithBuckets:(NSArray<NSNumber *> * _Nonnull)buckets backgroundImages:(NSArray<UIImage *> * _Nonnull)backgroundImages;
        [Export("initWithBuckets:backgroundImages:")]
        NativeHandle Constructor(NSNumber[] buckets, UIImage[] backgroundImages);

        // -(instancetype _Nonnull)initWithBuckets:(NSArray<NSNumber *> * _Nonnull)buckets backgroundColors:(NSArray<UIColor *> * _Nonnull)backgroundColors;
        [Export("initWithBuckets:backgroundColors:")]
        NativeHandle Constructor(NSNumber[] buckets, UIColor[] backgroundColors);

        // -(UIImage * _Nonnull)iconForSize:(NSUInteger)size;
        [Export("iconForSize:")]
        UIImage IconForSize(nuint size);
    }

    // @protocol GMUClusterRendererDelegate <NSObject>
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface GMUClusterRendererDelegate
    {
        // @optional -(GMSMarker * _Nullable)renderer:(id<GMUClusterRenderer> _Nonnull)renderer markerForObject:(id _Nonnull)object;
        [Export("renderer:markerForObject:")]
        [return: NullAllowed]
        GMSMarker MarkerForObject(IGMUClusterRenderer renderer, NSObject @object);

        // @optional -(void)renderer:(id<GMUClusterRenderer> _Nonnull)renderer willRenderMarker:(GMSMarker * _Nonnull)marker;
        [Export("renderer:willRenderMarker:")]
        void WillRenderMarker(IGMUClusterRenderer renderer, GMSMarker marker);

        // @optional -(void)renderer:(id<GMUClusterRenderer> _Nonnull)renderer didRenderMarker:(GMSMarker * _Nonnull)marker;
        [Export("renderer:didRenderMarker:")]
        void DidRenderMarker(IGMUClusterRenderer renderer, GMSMarker marker);
    }

    interface IGMUClusterRendererDelegate { }

    // @interface GMUDefaultClusterRenderer : NSObject <GMUClusterRenderer>
    [BaseType(typeof(NSObject))]
    interface GMUDefaultClusterRenderer : GMUClusterRenderer
    {
        // -(instancetype _Nonnull)initWithMapView:(GMSMapView * _Nonnull)mapView clusterIconGenerator:(id<GMUClusterIconGenerator> _Nonnull)iconGenerator;
        [Export("initWithMapView:clusterIconGenerator:")]
        NativeHandle Constructor(GMSMapView mapView, IGMUClusterIconGenerator iconGenerator);

        // @property (nonatomic) BOOL animatesClusters;
        [Export("animatesClusters")]
        bool AnimatesClusters { get; set; }

        // @property (nonatomic) NSUInteger minimumClusterSize;
        [Export("minimumClusterSize")]
        nuint MinimumClusterSize { get; set; }

        // @property (nonatomic) NSUInteger maximumClusterZoom;
        [Export("maximumClusterZoom")]
        nuint MaximumClusterZoom { get; set; }

        // @property (nonatomic) double animationDuration;
        [Export("animationDuration")]
        double AnimationDuration { get; set; }

        // @property (nonatomic) int zIndex;
        [Export("zIndex")]
        int ZIndex { get; set; }

        [Wrap("WeakDelegate")]
        [NullAllowed]
        IGMUClusterRendererDelegate Delegate { get; set; }

        // @property (nonatomic, weak) id<GMUClusterRendererDelegate> _Nullable delegate;
        [NullAllowed, Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        // @property (readonly, nonatomic) NSArray<GMSMarker *> * _Nonnull markers;
        [Export("markers")]
        GMSMarker[] Markers { get; }

        // -(BOOL)shouldRenderAsCluster:(id<GMUCluster> _Nonnull)cluster atZoom:(float)zoom;
        [Export("shouldRenderAsCluster:atZoom:")]
        bool ShouldRenderAsCluster(IGMUCluster cluster, float zoom);
    }

    // @protocol GMUGeometry <NSObject>
    /*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
    [Protocol]
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
        NativeHandle Constructor(string styleID, [NullAllowed] UIColor strokeColor, [NullAllowed] UIColor fillColor, nfloat width, nfloat scale, nfloat heading, CGPoint anchor, [NullAllowed] string iconUrl, [NullAllowed] string title, bool hasFill, bool hasStroke);
    }

    // @protocol GMUGeometryContainer <NSObject>
    /*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
    [Protocol]
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
    interface GMUFeature : GMUGeometryContainer
    {
        // @property (readonly, nonatomic) NSString * _Nullable identifier;
        [NullAllowed, Export("identifier")]
        string Identifier { get; }

        // @property (readonly, nonatomic) NSDictionary<NSString *,NSObject *> * _Nullable properties;
        [NullAllowed, Export("properties")]
        NSDictionary<NSString, NSObject> Properties { get; }

        // @property (readonly, nonatomic) GMSCoordinateBounds * _Nullable boundingBox;
        [NullAllowed, Export("boundingBox")]
        GMSCoordinateBounds BoundingBox { get; }

        // -(instancetype _Nonnull)initWithGeometry:(id<GMUGeometry> _Nonnull)geometry identifier:(NSString * _Nullable)identifier properties:(NSDictionary<NSString *,NSObject *> * _Nullable)properties boundingBox:(GMSCoordinateBounds * _Nullable)boundingBox;
        [Export("initWithGeometry:identifier:properties:boundingBox:")]
        NativeHandle Constructor(IGMUGeometry geometry, [NullAllowed] string identifier, [NullAllowed] NSDictionary<NSString, NSObject> properties, [NullAllowed] GMSCoordinateBounds boundingBox);
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
        NativeHandle Constructor(NSUrl url);

        // -(instancetype _Nonnull)initWithData:(NSData * _Nonnull)data;
        [Export("initWithData:")]
        NativeHandle Constructor(NSData data);

        // -(instancetype _Nonnull)initWithStream:(NSInputStream * _Nonnull)stream;
        [Export("initWithStream:")]
        NativeHandle Constructor(NSInputStream stream);

        // -(void)parse;
        [Export("parse")]
        void Parse();
    }

    // @interface GMUGeometryCollection : NSObject <GMUGeometry>
    [BaseType(typeof(NSObject))]
    interface GMUGeometryCollection : GMUGeometry
    {
        // @property (readonly, nonatomic) NSArray<id<GMUGeometry>> * _Nonnull geometries;
        [Export("geometries")]
        IGMUGeometry[] Geometries { get; }

        // -(instancetype _Nonnull)initWithGeometries:(NSArray<id<GMUGeometry>> * _Nonnull)geometries;
        [Export("initWithGeometries:")]
        NativeHandle Constructor(IGMUGeometry[] geometries);
    }

    // @interface GMUPair : NSObject
    [BaseType(typeof(NSObject))]
    interface GMUPair
    {
        // @property (readonly, nonatomic) NSString * _Nonnull key;
        [Export("key")]
        string Key { get; }

        // @property (readonly, nonatomic) NSString * _Nonnull styleUrl;
        [Export("styleUrl")]
        string StyleUrl { get; }

        // -(instancetype _Nonnull)initWithKey:(NSString * _Nonnull)styleID styleUrl:(NSString * _Nonnull)strokeColor;
        [Export("initWithKey:styleUrl:")]
        NativeHandle Constructor(string styleID, string strokeColor);
    }

    // @interface GMUStyleMap : NSObject
    [BaseType(typeof(NSObject))]
    interface GMUStyleMap
    {
        // @property (readonly, nonatomic) NSString * _Nonnull styleMapId;
        [Export("styleMapId")]
        string StyleMapId { get; }

        // @property (readonly, nonatomic) NSArray<GMUPair *> * _Nonnull pairs;
        [Export("pairs")]
        GMUPair[] Pairs { get; }

        // -(instancetype _Nonnull)initWithId:(NSString * _Nonnull)styleMapId pairs:(NSArray<GMUPair *> * _Nonnull)pairs;
        [Export("initWithId:pairs:")]
        NativeHandle Constructor(string styleMapId, GMUPair[] pairs);
    }

    // @interface GMUGeometryRenderer : NSObject
    [BaseType(typeof(NSObject))]
    interface GMUGeometryRenderer
    {
        // -(instancetype _Nonnull)initWithMap:(GMSMapView * _Nonnull)map geometries:(NSArray<id<GMUGeometryContainer>> * _Nonnull)geometries;
        [Export("initWithMap:geometries:")]
        NativeHandle Constructor(GMSMapView map, IGMUGeometryContainer[] geometries);

        // -(instancetype _Nonnull)initWithMap:(GMSMapView * _Nonnull)map geometries:(NSArray<id<GMUGeometryContainer>> * _Nonnull)geometries styles:(NSArray<GMUStyle *> * _Nullable)styles;
        [Export("initWithMap:geometries:styles:")]
        NativeHandle Constructor(GMSMapView map, IGMUGeometryContainer[] geometries, [NullAllowed] GMUStyle[] styles);

        // -(instancetype _Nonnull)initWithMap:(GMSMapView * _Nonnull)map geometries:(NSArray<id<GMUGeometryContainer>> * _Nonnull)geometries styles:(NSArray<GMUStyle *> * _Nullable)styles styleMaps:(NSArray<GMUStyleMap *> * _Nullable)styleMaps;
        [Export("initWithMap:geometries:styles:styleMaps:")]
        NativeHandle Constructor(GMSMapView map, IGMUGeometryContainer[] geometries, [NullAllowed] GMUStyle[] styles, [NullAllowed] GMUStyleMap[] styleMaps);

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
        NativeHandle Constructor(UIColor[] colors, NSNumber[] startPoints, nuint mapSize);

        // -(NSArray<UIColor *> * _Nonnull)generateColorMap;
        [Export("generateColorMap")]
        //[Verify(MethodToProperty)]
        UIColor[] GenerateColorMap { get; }
    }

    // @interface GMUGridBasedClusterAlgorithm : NSObject <GMUClusterAlgorithm>
    [BaseType(typeof(NSObject))]
    interface GMUGridBasedClusterAlgorithm : GMUClusterAlgorithm
    {
    }

    // @interface GMUGroundOverlay : NSObject <GMUGeometry>
    [BaseType(typeof(NSObject))]
    interface GMUGroundOverlay : GMUGeometry
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
        NativeHandle Constructor(CLLocationCoordinate2D northEast, CLLocationCoordinate2D southWest, int zIndex, double rotation, string href);
    }

    // @protocol GQTPointQuadTreeItem <NSObject>
    /*
  Check whether adding [Model] to this declaration is appropriate.
  [Model] is used to generate a C# class that implements this protocol,
  and might be useful for protocols that consumers are supposed to implement,
  since consumers can subclass the generated class instead of implementing
  the generated interface. If consumers are not supposed to implement this
  protocol, then [Model] is redundant and will generate code that will never
  be used.
*/
    [Protocol]
    [BaseType(typeof(NSObject))]
    interface GQTPointQuadTreeItem
    {
        // @required -(GQTPoint)point;
        [Abstract]
        [Export("point")]
        //[Verify(MethodToProperty)]
        GQTPoint Point { get; }
    }

    interface IGQTPointQuadTreeItem { }

    // @interface GMUWeightedLatLng : NSObject <GQTPointQuadTreeItem>
    [BaseType(typeof(NSObject))]
    interface GMUWeightedLatLng : GQTPointQuadTreeItem
    {
        // @property (readonly, nonatomic) float intensity;
        [Export("intensity")]
        float Intensity { get; }

        // -(instancetype _Nonnull)initWithCoordinate:(CLLocationCoordinate2D)coordinate intensity:(float)intensity;
        [Export("initWithCoordinate:intensity:")]
        NativeHandle Constructor(CLLocationCoordinate2D coordinate, float intensity);
    }

    // @interface GMUHeatmapTileLayer : GMSSyncTileLayer
    [BaseType(typeof(GMSSyncTileLayer))]
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

        // @property (readonly, nonatomic) NSArray<GMUStyleMap *> * _Nonnull styleMaps;
        [Export("styleMaps")]
        GMUStyleMap[] StyleMaps { get; }

        // -(void)parse;
        [Export("parse")]
        void Parse();

        // -(instancetype _Nonnull)initWithURL:(NSURL * _Nonnull)url;
        [Export("initWithURL:")]
        NativeHandle Constructor(NSUrl url);

        // -(instancetype _Nonnull)initWithData:(NSData * _Nonnull)data;
        [Export("initWithData:")]
        NativeHandle Constructor(NSData data);

        // -(instancetype _Nonnull)initWithStream:(NSInputStream * _Nonnull)stream;
        [Export("initWithStream:")]
        NativeHandle Constructor(NSInputStream stream);
    }

    // @interface GMULineString : NSObject <GMUGeometry>
    [BaseType(typeof(NSObject))]
    interface GMULineString : GMUGeometry
    {
        // @property (readonly, nonatomic) GMSPath * _Nonnull path;
        [Export("path")]
        GMSPath Path { get; }

        // -(instancetype _Nonnull)initWithPath:(GMSPath * _Nonnull)path;
        [Export("initWithPath:")]
        NativeHandle Constructor(GMSPath path);
    }

    // @interface GMUNonHierarchicalDistanceBasedAlgorithm : NSObject <GMUClusterAlgorithm>
    [BaseType(typeof(NSObject))]
    interface GMUNonHierarchicalDistanceBasedAlgorithm : GMUClusterAlgorithm
    {
        // -(instancetype)initWithClusterDistancePoints:(NSUInteger)clusterDistancePoints;
        [Export("initWithClusterDistancePoints:")]
        NativeHandle Constructor(nuint clusterDistancePoints);
    }

    // @interface GMUPlacemark : NSObject <GMUGeometryContainer>
    [BaseType(typeof(NSObject))]
    interface GMUPlacemark : GMUGeometryContainer
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
        NativeHandle Constructor([NullAllowed] IGMUGeometry geometry, [NullAllowed] string title, [NullAllowed] string snippet, [NullAllowed] GMUStyle style, [NullAllowed] string styleUrl);
    }

    // @interface GMUPoint : NSObject <GMUGeometry>
    [BaseType(typeof(NSObject))]
    interface GMUPoint : GMUGeometry
    {
        // @property (readonly, nonatomic) CLLocationCoordinate2D coordinate;
        [Export("coordinate")]
        CLLocationCoordinate2D Coordinate { get; }

        // -(instancetype _Nonnull)initWithCoordinate:(CLLocationCoordinate2D)coordinate;
        [Export("initWithCoordinate:")]
        NativeHandle Constructor(CLLocationCoordinate2D coordinate);
    }

    // @interface GMUPolygon : NSObject <GMUGeometry>
    [BaseType(typeof(NSObject))]
    interface GMUPolygon : GMUGeometry
    {
        // @property (readonly, nonatomic) NSArray<GMSPath *> * _Nonnull paths;
        [Export("paths")]
        GMSPath[] Paths { get; }

        // -(instancetype _Nonnull)initWithPaths:(NSArray<GMSPath *> * _Nonnull)paths;
        [Export("initWithPaths:")]
        NativeHandle Constructor(GMSPath[] paths);
    }

    // @interface GMUSimpleClusterAlgorithm : NSObject <GMUClusterAlgorithm>
    [BaseType(typeof(NSObject))]
    interface GMUSimpleClusterAlgorithm : GMUClusterAlgorithm
    {
    }

    // @interface GMUStaticCluster : NSObject <GMUCluster>
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface GMUStaticCluster : GMUCluster
    {
        // -(instancetype _Nonnull)initWithPosition:(CLLocationCoordinate2D)position __attribute__((objc_designated_initializer));
        [Export("initWithPosition:")]
        [DesignatedInitializer]
        NativeHandle Constructor(CLLocationCoordinate2D position);

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
        NativeHandle Constructor(NSObject @object);
    }

    // @interface GQTPointQuadTree : NSObject
    [BaseType(typeof(NSObject))]
    interface GQTPointQuadTree
    {
        // -(id)initWithBounds:(GQTBounds)bounds;
        [Export("initWithBounds:")]
        NativeHandle Constructor(GQTBounds bounds);

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
        //[Verify(StronglyTypedNSArray)]
        NSObject[] SearchWithBounds(GQTBounds bounds);

        // -(NSUInteger)count;
        [Export("count")]
        //[Verify(MethodToProperty)]
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

    [Static]
    //[Verify(ConstantsInterfaceAssociation)]
    partial interface Constants
    {
        // extern double GoogleMapsUtilsVersionNumber;
        [Field("GoogleMapsUtilsVersionNumber", "__Internal")]
        double GoogleMapsUtilsVersionNumber { get; }

        // extern const unsigned char [] GoogleMapsUtilsVersionString;
        //[Field("GoogleMapsUtilsVersionString", "__Internal")]
        //NativeHandle GoogleMapsUtilsVersionString { get; }
    }
}