using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PaymentSchduler.Models;

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
        public ActionResult Create(PaymentSchedule model)
        {
            var paymentSchedule = new PaymentSchedule
            {
                VehiclePrice = model.VehiclePrice,
                DepositAmount = model.DepositAmount,
                FinanceOption = model.FinanceOption,
                DeliveryDate = model.DeliveryDate
            };

            return RedirectToAction("Index", "Home");
        }
    }
}