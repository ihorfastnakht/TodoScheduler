using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoScheduler.Helpers
{
    public static class StringHelper
    {
        public static string DayCutter(string fullDayText)
        {
            return fullDayText.Substring(0, 3);
        }
    }
}
