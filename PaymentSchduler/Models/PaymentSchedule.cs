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

        public PaymentSchedule(PaymentScheduleViewModel paymentSchedule)
        {
            decimal requiredDepositMin = paymentSchedule.VehiclePrice * 0.15m;

            if (paymentSchedule.DepositAmount < requiredDepositMin)
                throw new Exception("Deposit must be 15% of vehicle price.");

            VehiclePrice = paymentSchedule.VehiclePrice;
            DepositAmount = paymentSchedule.DepositAmount;
            DeliveryDate = paymentSchedule.DeliveryDate;

            FinanceOptionInMonths = CalculateFinancialOptionInMonths(paymentSchedule.FinanceOption);
        }

        private int CalculateFinancialOptionInMonths(int option)
        {
            int months = 12;
            return option * months;
        }
        
    }
}