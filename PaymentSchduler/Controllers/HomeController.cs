using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PaymentSchduler.Models;
using PaymentSchduler.ViewModels;

namespace PaymentSchduler.Controllers
{
    public class HomeController : Controller
    {

        public readonly ApplicationDbContext _context;


        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PaymentScheduleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            PaymentSchedule paymentSchedule = new PaymentSchedule(viewModel);

            PreparePaymentSchedule(
                SubtractDepositFromFullPrice(paymentSchedule.VehiclePrice, paymentSchedule.DepositAmount),
                paymentSchedule.FinanceOptionInMonths,
                paymentSchedule.DeliveryDate);

            return View("Index", viewModel);
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

        private List<PaymentAndDate> PreparePaymentSchedule(decimal vehiclePrice, int financialOptionInMonths, DateTime deliveryDate)
        {
            List<PaymentAndDate> paymentScheduleBreakDown = new List<PaymentAndDate>();
            DateTime datePaymentsStart = FindPaymentStartDate(deliveryDate);
   
            decimal decreasingVehiclePrice = vehiclePrice;
            decimal monthlyPayment = CalculateMonthlyPayments(vehiclePrice, financialOptionInMonths);
            
            for (int month = 1; month <= financialOptionInMonths; month++)
            {
                //ArrangeMonthlyPayments
                PaymentAndDate paymentAndDate = new PaymentAndDate();
                DateTime firstMondayOfMonth;

                paymentAndDate.PaymentDate = FindFirstMondayOfMonth(datePaymentsStart, out firstMondayOfMonth);
                paymentAndDate.PaymentValue = monthlyPayment;
                           
                if (month == financialOptionInMonths)                
                    paymentAndDate.PaymentValue = EnsureScheduleHasBeenCompletelyPaid(decreasingVehiclePrice, paymentAndDate.PaymentValue);
                
                decreasingVehiclePrice -= paymentAndDate.PaymentValue;
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
    }
}