using System;
using System.Collections.Generic;
using PaymentSchduler.Models;


namespace PaymentSchduler.Domain
{
    public class CalculationHelper
    {
        public static List<PaymentAndDate> GetPaymentBreakDown()
        {
            List<PaymentAndDate> paymentBreakDown = new List<PaymentAndDate>();

            return paymentBreakDown;
        }

        private void PreparePaymentSchedule(List<PaymentAndDate> paymentBreakDown)
        {
            DateTime date = DateTime.Now;

            date = date.AddMonths(1);

            int start = DateTime.DaysInMonth(date.Year, date.Month) - date.Day;

            DateTime dateStarts = date.AddDays(start + 1);

            int finance = 3;

            for (int mth = 1; mth <= 12 * finance; mth++)
            {
                dateStarts = new DateTime(dateStarts.Year, dateStarts.Month, 1);

                while (dateStarts.DayOfWeek != DayOfWeek.Monday)
                {
                    dateStarts = dateStarts.AddDays(1);
                }

                if (dateStarts.DayOfWeek == DayOfWeek.Monday)
                {
                   //add
                    int next = DateTime.DaysInMonth(dateStarts.Year, dateStarts.Month) - dateStarts.Day;
                    dateStarts = dateStarts.AddDays(next + 1);
                }
            }
        }
        
        
    }
}