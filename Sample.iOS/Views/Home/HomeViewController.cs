using System;
using System.Collections.Generic;

using UIKit;

using Sample.iOS.Models;
using Sample.iOS.Views.KML;
using Sample.iOS.Views.Basic;
using Sample.iOS.Views.HeatMap;
using Sample.iOS.Views.GeoJson;
using Sample.iOS.Views.Home.Cell;
using Sample.iOS.Views.Home.Source;
using Sample.iOS.Views.CustomMarker;

namespace Sample.iOS.Views.Home
{
    public partial class HomeViewController : UIViewController
    {
        private List<Pages> pages;
        private HomeSource homeSource;

        public HomeViewController() : base("HomeViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetTableProperties();
            NavigationItem.Title = "Demos";
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            homeSource.ItemSelected += HomeSource_ItemSelected;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            homeSource.ItemSelected -= HomeSource_ItemSelected;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        private void SetTableProperties()
        {
            PopulatePages();
            homeSource = new HomeSource(pages);
            tableView.RegisterNibForCellReuse(HomeViewCell.Nib, HomeViewCell.Key);
            tableView.Source = homeSource;
        }

        private void PopulatePages()
        {
            pages = new List<Pages>()
            {
                new Pages("Basic","",new BasicViewController()),
                new Pages("Custom Marker","",new CustomMarkerViewController()),
                new Pages("Geo Json import","",new GeoJsonViewController()),
                new Pages("Heat Map import","",new HeatMapViewController()),
                new Pages("Kml Import","",new KMLViewController())
            };
        }

        void HomeSource_ItemSelected(int row)
        {
            NavigationController.PushViewController(pages[row].Page,true);
        }
    }
}