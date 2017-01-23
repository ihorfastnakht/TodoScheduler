using System;

namespace TodoScheduler.PageModel
{
    public class MenuGroup
    {
        public string Icon { get; set; }
        public string Title { get; set; }
    }

    public class MenuItem
    {
        public MenuGroup Group { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public Type ViewModelType { get; set; }
    }
}
