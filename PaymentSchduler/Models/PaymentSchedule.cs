using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PaymentSchduler.Models
{
    public class PaymentSchedule
    {
        public int Id { get; set; }
        [Required]
        public decimal DepositAmount { get; set; }

        [Required]
        public decimal VehiclePrice { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

        public FinanceOption FinanceOption { get; set; }

        public byte FinanceOptionId { get; set; }

        public int FinanceOp { get; set; }

        public decimal PaymentDue { get; set; }

    }
}