using System;
using System.Collections.Generic;
using PaymentSchduler.Models;

namespace PaymentSchduler.Domain
{
    public class PaymentPlanCreater
    {

        private decimal vehiclePrice;
        private DateTime datePaymentsStart;
        private decimal monthlyPayments;
        private int financeOptionInMonths;
        private decimal firstMonthArrangementFee;
        private decimal finalMonthArrangementFee;

        private DateGenerator datess;

        public PaymentPlanCreater(PaymentSchedule paymentSchedule)
        {
            datess = new DateGenerator(paymentSchedule.DeliveryDate, paymentSchedule.FinanceOptionInMonths);


            this.vehiclePrice = SubtractDepositFromFullPrice(paymentSchedule.VehiclePrice, paymentSchedule.DepositAmount);


            this.datePaymentsStart = FindPaymentStartDate(paymentSchedule.DeliveryDate);
            this.monthlyPayments = CalculateMonthlyPayments(vehiclePrice, paymentSchedule.FinanceOptionInMonths);
            this.financeOptionInMonths = paymentSchedule.FinanceOptionInMonths;
            this.firstMonthArrangementFee = paymentSchedule.FirstMonthArrangementFee;
            this.finalMonthArrangementFee = paymentSchedule.FinalMonthArrangementFee;
        }
        public List<PaymentAndDate> GeneratePlan()
        {
            var dates = datess.GenerateCalenderDates();

            dates.GenerateCalenderDates();

            return null;
        }


        private PaymentAndDate CreatePaymentAndDate(DateTime date, decimal value)
        {
            return new PaymentAndDate()
            {
                PaymentDate = date,
                PaymentValue = value

            };
        }
        private decimal SubtractMonthlyPaymentFromVehiclePrice(int month)
        {
            return (month == financeOptionInMonths) ? vehiclePrice -= vehiclePrice : vehiclePrice -= monthlyPayments;
        }

        private decimal CalculatePaymentValueForMonth(int month)
        {
            if (month == 1)
            {
                return monthlyPayments + firstMonthArrangementFee;
            }
            else if (month == financeOptionInMonths)
            {
                return EnsureScheduleHasBeenCompletelyPaid() + finalMonthArrangementFee;
            }
            else
            {
                return monthlyPayments;
            }
        }



        private decimal EnsureScheduleHasBeenCompletelyPaid()
        {
            return vehiclePrice > monthlyPayments || vehiclePrice < monthlyPayments ? vehiclePrice : monthlyPayments;
        }

        private DateTime FindPaymentStartDate(DateTime deliveryDate)
        {
            deliveryDate = deliveryDate.AddMonths(1);

            var start = DateTime.DaysInMonth(deliveryDate.Year, deliveryDate.Month) - deliveryDate.Day;

            return deliveryDate.AddDays(start + 1);
        }

        private DateTime FindFirstMondayOfMonth(DateTime datePaymentsStart, out DateTime firstMonday)
        {
            datePaymentsStart = new DateTime(datePaymentsStart.Year, datePaymentsStart.Month, 1);

            firstMonday = datePaymentsStart;

            datePaymentsStart = FindNextMonday(datePaymentsStart);

            if (datePaymentsStart.DayOfWeek == DayOfWeek.Monday)
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
            var followingMonth = DateTime.DaysInMonth(datePaymentsStart.Year, datePaymentsStart.Month) - datePaymentsStart.Day;
            return datePaymentsStart.AddDays(followingMonth + 1);
        }


        private decimal SubtractDepositFromFullPrice(decimal fullPrice, decimal depositAmount)
        {
            return fullPrice - depositAmount;
        }


        private decimal CalculateMonthlyPayments(decimal vehiclePrice, int financialOptionInMonths)
        {
            var decimalPlaces = 2;
            return Math.Round(vehiclePrice / financialOptionInMonths, decimalPlaces);
        }

        private decimal SubtractPaymentFromVehiclePrice(decimal vehiclePrice, decimal monthlyCost)
        {
            return monthlyCost;
        }

    }
}