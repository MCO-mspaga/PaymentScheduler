using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentSchduler.Domain
{
    public class DateGenerator
    {
        private DateTime startDate;
        private int duration;

        public DateGenerator(DateTime startDate, int duration)
        {
            this.startDate = startDate;
            this.duration = duration;
        }

        public List<DateTime> GenerateCalenderDates(string DayOfWeek)
        {
            startDate = FindStartDate();

            for (int month = 1; month <= duration; month++)
            {

                DateTime firstDayOfMonth;
                startDate = FindFirstDayOfMonth(startDate, out firstDayOfMonth);

                Dat

            }

        private DateTime FindStartDate()
        {
            startDate = startDate.AddMonths(1);

            int start = DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day;

            return startDate.AddDays(start + 1);
        }


        private DateTime FindFirstDayOfMonth(DateTime datePaymentsStart, out DateTime firstMonday)
        {
            datePaymentsStart = new DateTime(datePaymentsStart.Year, datePaymentsStart.Month, 1);

            firstMonday = datePaymentsStart;

            datePaymentsStart = FindNextMonday(datePaymentsStart);

            if (datePaymentsStart.DayOfWeek == )
            {
                firstMonday = datePaymentsStart;
                datePaymentsStart = MoveToNextMonth(datePaymentsStart);
            }

            return datePaymentsStart;
        }



        private DateTime FindNextMonday(DateTime datePaymentsStart)
        {
            while (datePaymentsStart.DayOfWeek != DayOfWeek.Monday)
            {
                datePaymentsStart = datePaymentsStart.AddDays(1);
            }
            return datePaymentsStart;
        }

        private DateTime MoveToNextMonth(DateTime datePaymentsStart)
        {
            int followingMonth = DateTime.DaysInMonth(datePaymentsStart.Year, datePaymentsStart.Month) - datePaymentsStart.Day;
            return datePaymentsStart.AddDays(followingMonth + 1);
        }

    }
}