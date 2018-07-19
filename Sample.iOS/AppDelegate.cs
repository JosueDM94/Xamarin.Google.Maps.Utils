using UIKit;
using Foundation;
using Google.Maps;

using Sample.iOS.Views.Home;
using System;

namespace Sample.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        private string GoogleMapsApiKey = "";

        public override UIWindow Window { get; set; }
        public UINavigationController navigationController { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {           
            if (string.IsNullOrEmpty(GoogleMapsApiKey))
                throw new Exception("Please provide your own Google Maps Api Key");
            

            Window = new UIWindow(UIScreen.MainScreen.Bounds);
            MapServices.ProvideAPIKey(GoogleMapsApiKey);
            navigationController = new UINavigationController(new HomeViewController());           
            Window.RootViewController = navigationController;
            Window.MakeKeyAndVisible();
            return true;
        }
    }
}