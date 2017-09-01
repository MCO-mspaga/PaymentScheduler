using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSchduler.Domain
{
    public class DateFinder
    {
        public static DateTime FindDate(DayOfWeek dayOfWeek, ref DateTime date)
        {
            var dateFound = FindDateForFollowingDaySpecified(dayOfWeek, date);

            date = MoveToNextMonth(date);

            return dateFound;
        }

        private static DateTime FindDateForFollowingDaySpecified(DayOfWeek dayOfWeek, DateTime date)
        {
            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1);
            }
            return date;
        }

        private static DateTime MoveToNextMonth(DateTime date)
        {
            var followingMonth = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;
            return date.AddDays(followingMonth + 1); ;
        }
    }
}
