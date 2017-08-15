using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentSchduler.ViewModels
{
    public class PaymentScheduleViewModel
    {
                [Required]
        [Display(Name = "Deposit Amount")]
        public decimal DepositAmount { get;  set; }

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