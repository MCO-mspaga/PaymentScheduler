using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSchduler.Domain
{
    public class MonthlyPayments
    {
        private decimal value;
        private int duration;
        private const float DECIMALPLACES = 2;

        public List<decimal> Payment
        {
            get;
            private set;
        }


        public MonthlyPayments(decimal value, int duration)
        {
            Payment = new List<decimal>(duration);
            this.value = value;
            this.duration = duration;
        }

        public void CalcPaymentsForTerm(decimal firstPayment, decimal lastPayment)
        {
            CalcStandardPayment();

            CalcFirstMonth(firstPayment);

            CalcLastMonth(lastPayment);
        }

        private void CalcStandardPayment()
        {
            Payment.AddRange(
                Enumerable.Repeat(
                    Math.Round(value / duration, decimalPlaces), duration));            
        }

        private void CalcFirstMonth(decimal firstPayment)
        {
            Payment[0] += firstPayment;
        }

        private void CalcLastMonth(decimal lastPayment)
        {
            Payment[Payment.Count - 1] += lastPayment;
        }
    }
}
