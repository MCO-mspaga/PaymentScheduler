using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentSchduler.Models
{
    public class Loan
    {
        public PaymentSchedule Schedule { get; set; }

        public List<PaymentAndDate> PaymentDates { get; set; }

    }
}