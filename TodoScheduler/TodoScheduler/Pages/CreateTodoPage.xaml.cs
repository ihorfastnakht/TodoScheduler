using System.Threading;
using System.Threading.Tasks;
using TodoScheduler.Controls;
using Xamarin.Forms;

namespace TodoScheduler.Pages
{
    public partial class CreateTodoPage : BasePage
    {
        public CreateTodoPage()
        {
            InitializeComponent();

            @switch.Toggled += (s, e) => ResizeAnimation();
        }

        private void ResizeAnimation()
        {
            Device.BeginInvokeOnMainThread(async() =>
            {
                if (@switch.IsToggled)
                {
                    container.HeightRequest = 130;
                    holder.IsVisible = true;
                }
                else
                {
                    container.HeightRequest = 32;
                    holder.IsVisible = false;
                }
                await scroller.ScrollToAsync(container.Bounds.X, container.Bounds.Y, false);
                ForceLayout();
            });
        }
    }
}
