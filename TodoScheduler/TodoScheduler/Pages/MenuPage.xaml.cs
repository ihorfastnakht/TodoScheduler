using TodoScheduler.Infrastructure.Base;

namespace TodoScheduler.Pages
{
    public partial class MenuPage : BaseContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            listView.ItemSelected += (s, e) => listView.SelectedItem = null;
        }
    }
}
