using System;
using System.Threading;
using System.Threading.Tasks;
using TodoScheduler.Controls;
using Xamarin.Forms;

namespace TodoScheduler.Pages
{
    public partial class CreateTodoPage : BasePage
    {
        bool isExpanded = false;

        public CreateTodoPage()
        {
            InitializeComponent();
            @switch.Toggled += (s, e) => ResizeAnimation();
        }

        private void ResizeAnimation()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Action<double> callback = async (input) => 
                {
                    container.HeightRequest = input;
                    await scroller.ScrollToAsync(container.Bounds.Left, this.Bounds.Bottom, false);
                    isExpanded = !isExpanded;
                    @switch.IsEnabled = true;
                    holder.IsVisible = true;
                };

                holder.IsVisible = false;
                @switch.IsEnabled = false;
                double startingHeight = container.Height;
                double endingHeight;
                Easing easing;// = Easing.CubicIn;

                if (@switch.IsToggled)
                {
                    endingHeight = 130;
                    easing = Easing.CubicOut;

                }
                else
                {
                    endingHeight = 32;
                    easing = Easing.CubicIn;
                }

                uint rate = 1;
                uint length = 400;

                container.Animate("invis", callback, startingHeight, endingHeight, rate, length, easing);
            });
        }
    }
}
