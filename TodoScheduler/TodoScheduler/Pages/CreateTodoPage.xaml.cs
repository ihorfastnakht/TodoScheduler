using TodoScheduler.Controls;

namespace TodoScheduler.Pages
{
    public partial class CreateTodoPage : BasePage
    {
        public CreateTodoPage()
        {
            InitializeComponent();

            //TODO: scroll to end

            @switch.Toggled += (s, e) =>
            {
                if (e.Value)
                {
                    reminderContainer.HeightRequest = 130;
                    holder.IsVisible = true;
                }
                else
                {
                    reminderContainer.HeightRequest = 32;
                    holder.IsVisible = false;
                }
                //ForceLayout();
            };
        }
    }
}
