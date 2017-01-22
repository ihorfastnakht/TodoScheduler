using Xamarin.Forms;

namespace TodoScheduler.Controls
{
    public class Separator : BoxView
    {
        #region bindable properties

        public static readonly BindableProperty SeparatorOrientationProperty = BindableProperty.Create("SeparatorOrientation",
            typeof(SeparatorOrientation), typeof(Separator), propertyChanged: OnChanged);

        #endregion

        #region property

        public SeparatorOrientation SeperatorOrientation
        {
            get { return (SeparatorOrientation)GetValue(SeparatorOrientationProperty); }
            set { SetValue(SeparatorOrientationProperty, value); }
        }

        #endregion

        public Separator()
        {

        }

        private static void OnChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var separator = (Separator)bindable;

            if (separator.SeperatorOrientation == SeparatorOrientation.Horizontal)
            {
                separator.HorizontalOptions = LayoutOptions.StartAndExpand;
                separator.VerticalOptions = LayoutOptions.Center;
            }
            else
            {
                separator.HorizontalOptions = LayoutOptions.Center;
                separator.VerticalOptions = LayoutOptions.StartAndExpand;
            }
        }
    }

    public enum SeparatorOrientation
    {
        Horizontal,
        Vertical
    }
}
