using System;

namespace TodoScheduler.PageModels
{
    public class MenuItem
    {
        public MenuGroup MenuGroup { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public Type ViewModelType;
    }
}
