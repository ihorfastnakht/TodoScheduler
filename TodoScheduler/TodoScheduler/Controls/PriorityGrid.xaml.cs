using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoScheduler.Enums;
using Xamarin.Forms;

namespace TodoScheduler.Controls
{
    public partial class PriorityGrid : Grid
    {
        public PriorityGrid()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty PriorityProperty = BindableProperty.Create("Priority", typeof(TodoPriority), typeof(PriorityGrid), TodoPriority.Low, defaultBindingMode: BindingMode.TwoWay);

        public TodoPriority Priority
        {
            get { return (TodoPriority)GetValue(PriorityProperty); }
            set
            {
                SetValue(PriorityProperty, value);
                UpdatePriority();
            }
        }

        private void UpdatePriority()
        {
            ClearOldState();

            if (Priority == TodoPriority.Low)
                low.IsVisible = true;
            else if (Priority == TodoPriority.Normal)
            {
                low.IsVisible = true;
                normal.IsVisible = true;
            }
            else if(Priority == TodoPriority.High)
            {
                low.IsVisible = true;
                normal.IsVisible = true;
                high.IsVisible = true;
            }
        }

        private void ClearOldState()
        {
            low.IsVisible = false;
            normal.IsVisible = false;
            high.IsVisible = false;
        }
    }
}
