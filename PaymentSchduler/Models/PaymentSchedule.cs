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
        public int Id { get; set; }
        [Required]
        [Display(Name = "Deposit Amount")]
        public decimal DepositAmount { get; private set; }

        [Display(Name = "Vehicle Price")]
        [Required(ErrorMessage = "Value should be at least 15% of Vehicle Price")]
        public decimal VehiclePrice { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Delivery Date")]
        [FutureDate(ErrorMessage = "Date should be in the future.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Financial Option")]
        public int FinanceOption { get; set; }               



        public void SetDepositAmount(decimal deposit)
        {
            decimal requiredDepositMin = VehiclePrice * 0.15m; 

            if(deposit >= requiredDepositMin)
            {
                DepositAmount = deposit;
            }
        }
    }
}