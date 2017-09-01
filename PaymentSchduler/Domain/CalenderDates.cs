using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSchduler.Domain
{
    public class CalenderDates
    {
        public List<DateTime> Dates
        {
            get; private set;
        }
        private int duration;
        private DateTime startDate;        

        public CalenderDates(int duration, DateTime startDate)
        {
            Dates = new List<DateTime>(duration);
            this.startDate = startDate;
            this.duration = duration;
        }

        public void GenerateDatesForTerm(DayOfWeek dayOfWeek)
        {
            Enumerable.Range(0, duration).ToList().ForEach(arg => Dates.Add(DateFinder.FindDate(dayOfWeek, ref startDate)));
        }
    }
}
