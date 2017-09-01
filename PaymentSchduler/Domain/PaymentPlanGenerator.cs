using System;
using System.Collections.Generic;
using PaymentSchduler.Models;
using PaymentSchduler.Domain.Interface;
using System.Linq;


namespace PaymentSchduler.Domain
{
    public class PaymentPlanGenerator : IPaymentPlanGenerator
    {
        private decimal firstMonthArrangementFee;
        private decimal finalMonthArrangementFee;
        private CalenderDates paymentDates;
        private MonthlyPayments monthlyPayments;


        public PaymentPlanGenerator(PaymentSchedule paymentSchedule)
        {
            this.paymentDates = new CalenderDates(paymentSchedule.FinanceOptionInMonths, FindPaymentStartDate(paymentSchedule.DeliveryDate));

            this.monthlyPayments = new MonthlyPayments(
                SubtractDepositFromFullPrice(
                    paymentSchedule.VehiclePrice,
                    paymentSchedule.DepositAmount),
                paymentSchedule.FinanceOptionInMonths);

            this.firstMonthArrangementFee = paymentSchedule.FirstMonthArrangementFee;
            this.finalMonthArrangementFee = paymentSchedule.FinalMonthArrangementFee;
        }

        public List<PaymentAndDate> GeneratePlan()
        {
            paymentDates.GenerateDatesForTerm(DayOfWeek.Monday);

            monthlyPayments.CalcPaymentsForTerm(firstMonthArrangementFee, finalMonthArrangementFee);
            
            return CreatePlan();
        }

        private List<PaymentAndDate> CreatePlan()
        {
            //this method currently does not work as it is returning a Enumerable<ZipIterator> instead of what a PaymentAndDate List.
            return (List<PaymentAndDate>)paymentDates.Dates.Zip(monthlyPayments.Payment, (d, p) => new PaymentAndDate() { PaymentDate = d, PaymentValue = p }); ;
        }

        private DateTime FindPaymentStartDate(DateTime deliveryDate)
        {
            deliveryDate = deliveryDate.AddMonths(1);

            var start = DateTime.DaysInMonth(deliveryDate.Year, deliveryDate.Month) - deliveryDate.Day;

            return deliveryDate.AddDays(start + 1);
        }

        private decimal SubtractDepositFromFullPrice(decimal fullPrice, decimal depositAmount)
        {
            return fullPrice - depositAmount;
        }
    }
}
