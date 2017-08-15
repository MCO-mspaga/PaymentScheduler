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

            ArrangeMonthlyPayments(
                SubtractDepositFromFullPrice(viewModel.VehiclePrice, viewModel.DepositAmount),
                paymentSchedule.FinanceOptionInMonths);

            return View("Index", viewModel);
        }

        private decimal SubtractDepositFromFullPrice(decimal fullPrice, decimal depositAmount)
        {
            return fullPrice - depositAmount;
        }

        private List<decimal> ArrangeMonthlyPayments(decimal vehiclePrice, int financialOptionInMonths)
        {
            List<decimal> monthlyPayments = new List<decimal>();

            decimal decreasingVehiclePrice = vehiclePrice;

            for (int month = 1; month <= financialOptionInMonths; month++)
            {
                decimal monthlyCost = CalculateMonthlyPayments(vehiclePrice, financialOptionInMonths);

                if (month == 1)
                {
                    decimal num = Math.Round(vehiclePrice % financialOptionInMonths, 2);
                }

                if (month == financialOptionInMonths)
                {
                    if (decreasingVehiclePrice > monthlyCost || decreasingVehiclePrice < monthlyCost)
                    {
                        monthlyCost = decreasingVehiclePrice;
                        decreasingVehiclePrice -= decreasingVehiclePrice;
                    }
                    else
                    {
                        decreasingVehiclePrice -= monthlyCost;
                    }
                }
                else
                {
                    decreasingVehiclePrice -= monthlyCost;
                }

                monthlyPayments.Add(monthlyCost);
            }
            return monthlyPayments;
        }

        private decimal CalculateMonthlyPayments(decimal vehiclePrice, int financialOptionInMonths)
        {
            return Math.Round(vehiclePrice / financialOptionInMonths, 2);
        }

        private decimal SubtractPaymentFromVehiclePrice(decimal vehiclePrice, decimal monthlyCost)
        {
            return monthlyCost;
        }

        private void PreparePaymentSchedule(decimal vehiclePrice, int financialOptionInMonths, DateTime deliveryDate)
        {
            List<PaymentAndDate> paymentScheduleBreakDown = new List<PaymentAndDate>();
            DateTime datePaymentsStart = FindPaymentStartDate(deliveryDate);
            // this is for payments
            decimal decreasingVehiclePrice = vehiclePrice;
            decimal monthlyPayment = CalculateMonthlyPayments(vehiclePrice, financialOptionInMonths);
            //////

            for (int month = 1; month <= financialOptionInMonths; month++)
            {
                PaymentAndDate paymentAndDate = new PaymentAndDate();

                DateTime firstMondayOfMonth;

                paymentAndDate.PaymentDate = FindFirstMondayOfMonth(datePaymentsStart, out firstMondayOfMonth);
                
                //for payments 
                paymentAndDate.PaymentValue = monthlyPayment;
           
                if (month == financialOptionInMonths)
                {
                    if (decreasingVehiclePrice > paymentAndDate.PaymentValue || decreasingVehiclePrice < paymentAndDate.PaymentValue)
                    {
                        paymentAndDate.PaymentValue = decreasingVehiclePrice;
                        decreasingVehiclePrice -= decreasingVehiclePrice;
                    }
                    else
                    {
                        decreasingVehiclePrice -= paymentAndDate.PaymentValue;
                    }
                }
                else
                {
                    decreasingVehiclePrice -= paymentAndDate.PaymentValue;
                }

                paymentScheduleBreakDown.Add(paymentAndDate);

            }
        }

        private decimal EnsureScheduleHasBeenCompletelyPaid(decimal decreasingVehiclePrice, decimal paymentValue)
        {
            if (decreasingVehiclePrice > paymentValue || decreasingVehiclePrice < paymentValue)
            {
                paymentValue = decreasingVehiclePrice;
                decreasingVehiclePrice -= decreasingVehiclePrice;
            }
            else
            {
                decreasingVehiclePrice -= paymentValue;
            }

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