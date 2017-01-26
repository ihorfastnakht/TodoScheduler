using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoScheduler.Base;

namespace TodoScheduler.ViewModels
{
    public class TomorrowViewModel : ViewModelBase
    {
        public override void Init(Dictionary<string, object> parameters = null)
        {
            base.Init(parameters);
            Header = $"Tomorrow ({DateTime.Now.AddDays(1).DayOfWeek}, {DateTime.Now.AddDays(1).ToString("dd.MM.yyyy")})";
        }
    }
}
