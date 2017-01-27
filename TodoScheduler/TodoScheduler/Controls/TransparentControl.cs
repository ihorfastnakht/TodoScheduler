using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TodoScheduler.Controls
{
    public class TransparentControl : StackLayout
    {
        public TransparentControl()
        {
            this.BackgroundColor = new Color(0, 0, 0, 0.5);
        }
    }
}
