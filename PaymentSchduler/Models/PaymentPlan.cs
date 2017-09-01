using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentSchduler.Models
{
    public class MonthlyPayments
    {
        private List<decimal> payments;
        private decimal value;
        private int duration;


        public MonthlyPayments(decimal value, int duration)
        {
            payments = new List<decimal>();
            this.value = value;
            this.duration = duration;
        }

        public void CalcPaymentsForTerm(decimal firstPayment, decimal lastPayment)
        {
            CalcStandardPayment();

            CalcFirstMonth(firstPayment);


        }

        private void CalcStandardPayment()
        {
            int decimalPlaces = 2;
            payments.Add(Math.Round(value / duration, decimalPlaces));
        }

        private void CalcFirstMonth(decimal firstPayment)
        {
            payments[0] += firstPayment;            
        }

        private void CalcLastMonth(decimal lastPayment)
        {
            payments[payments.Count-1] += lastPayment;
        }
    }
}