using System;
using System.Collections.Generic;
using PaymentSchduler.Models;
using PaymentSchduler.ViewModels;
using PaymentSchduler.Domain.Interface;


namespace PaymentSchduler.Domain
{
    public class PaymentPlanGenerator : IPaymentPlanGenerator
    {
        private decimal vehiclePrice;
        private DateTime datePaymentsStart;
        private decimal monthlyPayments;
        private int financeOptionInMonths;
        private decimal firstMonthArrangementFee;
        private decimal finalMonthArrangementFee;


        public PaymentPlanGenerator(PaymentSchedule paymentSchedule)
        {
            this.vehiclePrice = SubtractDepositFromFullPrice(paymentSchedule.VehiclePrice, paymentSchedule.DepositAmount);
            this.datePaymentsStart = FindPaymentStartDate(paymentSchedule.DeliveryDate);
            this.monthlyPayments = CalculateMonthlyPayments(vehiclePrice, paymentSchedule.FinanceOptionInMonths);
            this.financeOptionInMonths = paymentSchedule.FinanceOptionInMonths;
            this.firstMonthArrangementFee = paymentSchedule.FirstMonthArrangementFee;
            this.finalMonthArrangementFee = paymentSchedule.FinalMonthArrangementFee;
        }

        public List<PaymentAndDate> GeneratePlan()
        {
            List<PaymentAndDate> paymentScheduleBreakDown = new List<PaymentAndDate>();

            for (int month = 1; month <= financeOptionInMonths; month++)
            {
                DateTime firstMondayOfMonth;
                datePaymentsStart = FindFirstMondayOfMonth(datePaymentsStart, out firstMondayOfMonth);

                PaymentAndDate paymentAndDate = CreatePaymentAndDate(firstMondayOfMonth, CalculatePaymentValueForMonth(month));

                SubtractMonthlyPaymentFromVehiclePrice(month);

                paymentScheduleBreakDown.Add(paymentAndDate);
            }
            return paymentScheduleBreakDown;
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

            int start = DateTime.DaysInMonth(deliveryDate.Year, deliveryDate.Month) - deliveryDate.Day;

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
            int followingMonth = DateTime.DaysInMonth(datePaymentsStart.Year, datePaymentsStart.Month) - datePaymentsStart.Day;
            return datePaymentsStart.AddDays(followingMonth + 1);
        }


        private decimal SubtractDepositFromFullPrice(decimal fullPrice, decimal depositAmount)
        {
            return fullPrice - depositAmount;
        }


        private decimal CalculateMonthlyPayments(decimal vehiclePrice, int financialOptionInMonths)
        {
            int decimalPlaces = 2;
            return Math.Round(vehiclePrice / financialOptionInMonths, decimalPlaces);
        }

        private decimal SubtractPaymentFromVehiclePrice(decimal vehiclePrice, decimal monthlyCost)
        {
            return monthlyCost;
        }
    }
}
