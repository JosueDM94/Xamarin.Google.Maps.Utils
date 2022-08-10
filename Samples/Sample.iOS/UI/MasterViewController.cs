using System;
using System.Collections.Generic;
using Foundation;
using Sample.iOS.Models;
using Sample.iOS.Utils;
using UIKit;

namespace Sample.iOS
{
    public class MasterViewController : UITableViewController
    {
        private List<Pages> samples;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.Title = "Demos";
            samples = Samples.loadSamples();
            TableView.Source = new MasterSource(samples, this);
        }        

        public class MasterSource : UITableViewSource
        {
            private List<Pages> samples;
            private UIViewController parentController;

            public MasterSource(List<Pages> samples, UIViewController controller)
            {
                this.samples = samples;
                parentController = controller;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cellIdentifier = "Cell";
                var cell = tableView.DequeueReusableCell(identifier: cellIdentifier);
                if (cell == null)
                {
                    cell = new UITableViewCell(style: UITableViewCellStyle.Subtitle, reuseIdentifier: cellIdentifier);
                    cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                }

                cell.DetailTextLabel.Text = samples[indexPath.Row].Description;
                cell.TextLabel.Text = samples[indexPath.Row].Title;
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return samples.Count;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var sample = samples[indexPath.Row];
                var controller = Activator.CreateInstance(sample.Controller) as UIViewController;
                parentController.NavigationController.PushViewController(controller, animated: true);
            }
        }
    }
}
