using System;

namespace Sample.iOS.Models
{
    public class Pages
    {
        public Pages(string title, string description, Type controller)
        {
            Title = title;
            Description = description;
            Controller = controller;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public Type Controller { get; set; }
    }
}