using UIKit;

namespace Sample.iOS.Models
{
    public class Pages
    {
        public Pages(string title, string description, UIViewController page)
        {
            Title = title;
            Description = description;
            Page = page;
        }

        public string Title {  get; set; }
        public string Description { get; set; }
        public UIViewController Page { get; set; }
    }
}