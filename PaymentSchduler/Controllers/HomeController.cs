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

            for(int month = 1; month <= financialOptionInMonths; month++)
            {
                decimal monthlyCost = CalculateMonthlyPayments(vehiclePrice, financialOptionInMonths);
                
                if(month == 1)
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
    }
}