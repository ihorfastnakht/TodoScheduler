using Xamarin.Forms;

namespace TodoScheduler.Controls
{
    public class Separator : StackLayout
    {
        public Separator()
        {
            this.HeightRequest = 1;
            BackgroundColor = Color.FromHex("#808080");
        }
    }
}
