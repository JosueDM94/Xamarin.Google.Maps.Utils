using System;
using System.Collections.Generic;

using UIKit;
using Foundation;

using Sample.iOS.Models;
using Sample.iOS.Views.Home.Cell;

namespace Sample.iOS.Views.Home.Source
{
    public class HomeSource : UITableViewSource
    {
        public Action<int> ItemSelected { get; set; }
        
        private List<Pages> pages;
        public HomeSource(List<Pages> pages)
        {
            this.pages = pages;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(HomeViewCell.Key,indexPath);
            cell.DetailTextLabel.Text = pages[indexPath.Row].Description;
            cell.TextLabel.Text = pages[indexPath.Row].Title;
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return pages.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            ItemSelected?.Invoke(indexPath.Row);
        }
    }
}