﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using PaymentSchduler.Models;

namespace PaymentSchduler.ViewModels
{
    public class PaymentScheduleViewModel
    {
        [Required]
        [Display(Name = "Deposit Amount")]
        public decimal DepositAmount { get; set; } = 150;

        [Display(Name = "Vehicle Price")]
        [Required]
        public decimal VehiclePrice { get; set; } = 1000;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Delivery Date")]
        [FutureDate(ErrorMessage = "Date should be in the future.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; } = DateTime.Now;

        [Display(Name = "Financial Option")]
        public int FinanceOption { get; set; } = 1;
        [Display(Name = "Deposit Percetage")]
        public decimal? DepositPercentage { get; set; } 
        [Display(Name = "Frist Month Arrangement Fee")]
        public decimal? FirstMonthArrangementFee { get; set; } 
        [Display(Name = "Final Month Arrangement Fee")]
        public decimal? FinalMonthArrangementFee { get; set; } 

        public List<PaymentAndDate> PaymentDates { get; set; }
       
     
    }
}