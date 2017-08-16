using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PaymentSchduler.ViewModels;

namespace PaymentSchduler.Models
{
    public class PaymentSchedule
    {

        [Required]
        public decimal VehiclePrice { get; private set; }
        [Required]
        public decimal DepositAmount { get; private set; }

        [Required]
        public DateTime DeliveryDate { get; private set; }
        [Required]
        public int FinanceOptionInMonths { get; private set; }

        public decimal DepositPercentage { get; private set; } = 15m;

        public decimal FirstMonthArrangementFee { get; private set; } = 88m;

        public decimal FinalMonthArrangementFee { get; private set; } = 20m;

        public PaymentSchedule(PaymentScheduleViewModel paymentSchedule)
        {
            DepositPercentage /= 100;
            decimal requiredDepositMin = paymentSchedule.VehiclePrice * DepositPercentage;

            if (paymentSchedule.DepositAmount < requiredDepositMin)
                throw new Exception("Deposit must be " + DepositPercentage + "% of vehicle price.");

            VehiclePrice = paymentSchedule.VehiclePrice;
            DepositAmount = paymentSchedule.DepositAmount;
            DeliveryDate = paymentSchedule.DeliveryDate;
            DepositPercentage = paymentSchedule.DepositPercentage;
            FirstMonthArrangementFee = paymentSchedule.FirstMonthArrangementFee;
            FinalMonthArrangementFee = paymentSchedule.FinalMonthArrangementFee;

            FinanceOptionInMonths = CalculateFinancialOptionInMonths(paymentSchedule.FinanceOption);
        }

        private int CalculateFinancialOptionInMonths(int option)
        {
            int months = 12;
            return option * months;
        }
        
    }
}