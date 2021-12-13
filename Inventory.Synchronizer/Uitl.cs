using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Synchronizer
{
    public static class DateExtensions
    {
        public static DateTime ToEndOfDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);
        }
    }
}
