using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TodoScheduler.Controls
{
    public class ImageButton : Image
    {
        bool isPressed = false;

        public ImageButton()
        {
            this.Opacity = 0.5;

            var gestureRecognizer = new TapGestureRecognizer();

            gestureRecognizer.Tapped += (s, e) =>
            {
                if (isPressed)
                    return;

                isPressed = true;

                Device.BeginInvokeOnMainThread(async() => await Animate());

                if (Command != null)
                    Command.Execute(CommandParameter);

                isPressed = false;
            };

            this.GestureRecognizers.Add(gestureRecognizer);
        }

        #region bindable 

        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand),
            typeof(ImageButton));

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object),
            typeof(ImageButton));

        #endregion

        #region properties

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        #endregion


        public async Task Animate()
        {
            await this.ScaleTo(1.4, 75);
            await this.ScaleTo(1.0, 75);
        }
    }
}
