using Xamarin.Forms;

namespace TodoScheduler.Infrastructure.Base
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            if (Device.OS == TargetPlatform.iOS)
                this.Padding = new Thickness(0,20,0,0);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = this.BindingContext as ViewModelBase;

            if (viewModel != null)
                viewModel.Appearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}
