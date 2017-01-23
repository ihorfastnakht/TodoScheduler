using TodoScheduler.Controls;

namespace TodoScheduler.Pages
{
    public partial class MenuPage : BasePage
    {
        public MenuPage()
        {
            InitializeComponent();
            //HACK
            listView.ItemSelected += (s, e) => listView.SelectedItem = null;
        }
    }
}
