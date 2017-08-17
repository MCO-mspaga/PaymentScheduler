using System;
using System.Collections.Generic;
using PaymentSchduler.Models;
using PaymentSchduler.ViewModels;
using PaymentSchduler.Domain.Interface;


namespace PaymentSchduler.Domain
{
    public class LoanGenerator : ILoanGenerator
    {
        public PaymentScheduleViewModel GenerateLoan(PaymentScheduleViewModel viewModel)
        {
            PaymentSchedule paymentSchedule = new PaymentSchedule(viewModel);

            return viewModel.PrepareModelForDisplayingLoan(paymentSchedule, PreparePaymentSchedule(paymentSchedule));            
        }


        private List<PaymentAndDate> PreparePaymentSchedule(PaymentSchedule paymentSchedule)
        {
            List<PaymentAndDate> paymentScheduleBreakDown = new List<PaymentAndDate>();
            DateTime datePaymentsStart = FindPaymentStartDate(paymentSchedule.DeliveryDate);

            decimal decreasingVehiclePrice = SubtractDepositFromFullPrice(paymentSchedule.VehiclePrice, paymentSchedule.DepositAmount);
            decimal monthlyPayment = CalculateMonthlyPayments(decreasingVehiclePrice, paymentSchedule.FinanceOptionInMonths);

            for (int month = 1; month <= paymentSchedule.FinanceOptionInMonths; month++)
            {
                PaymentAndDate paymentAndDate = new PaymentAndDate();
                DateTime firstMondayOfMonth;

                paymentAndDate.PaymentDate = FindFirstMondayOfMonth(datePaymentsStart, out firstMondayOfMonth);
                paymentAndDate.PaymentValue = month == 1 ? monthlyPayment + paymentSchedule.FirstMonthArrangementFee : monthlyPayment;

                if (month == paymentSchedule.FinanceOptionInMonths)
                {
                    paymentAndDate.PaymentValue =
                        EnsureScheduleHasBeenCompletelyPaid(decreasingVehiclePrice,
                            paymentAndDate.PaymentValue) + paymentSchedule.FinalMonthArrangementFee;
                    decreasingVehiclePrice -= decreasingVehiclePrice;
                }
                else
                {
                    decreasingVehiclePrice -= monthlyPayment;
                }                                

               paymentScheduleBreakDown.Add(paymentAndDate);
            }
            return paymentScheduleBreakDown;
        }


        private decimal EnsureScheduleHasBeenCompletelyPaid(decimal decreasingVehiclePrice, decimal paymentValue)
        {
            return decreasingVehiclePrice > paymentValue || decreasingVehiclePrice < paymentValue ? decreasingVehiclePrice : paymentValue;
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

            while (datePaymentsStart.DayOfWeek != DayOfWeek.Monday)
            {
                datePaymentsStart = datePaymentsStart.AddDays(1);
            }

            if (datePaymentsStart.DayOfWeek == DayOfWeek.Monday)
            {
                firstMonday = datePaymentsStart;
                int next = DateTime.DaysInMonth(datePaymentsStart.Year, datePaymentsStart.Month) - datePaymentsStart.Day;
                datePaymentsStart = datePaymentsStart.AddDays(next + 1);
            }

            return datePaymentsStart;
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
