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
        public decimal DepositAmount { get; set; }

        [Display(Name = "Vehicle Price")]
        [Required]
        public decimal VehiclePrice { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Delivery Date")]
        [FutureDate(ErrorMessage = "Date should be in the future.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Financial Option")]
        public int FinanceOption { get; set; }               
    }
}