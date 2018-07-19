using System;

using UIKit;
using Foundation;

namespace Sample.iOS.Views.Home.Cell
{
    public partial class HomeViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("HomeViewCell");
        public static readonly UINib Nib;

        static HomeViewCell()
        {
            Nib = UINib.FromName("HomeViewCell", NSBundle.MainBundle);
        }

        protected HomeViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}