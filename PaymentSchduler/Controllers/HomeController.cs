using System.Web.Mvc;
using PaymentSchduler.Models;
using PaymentSchduler.ViewModels;
using PaymentSchduler.Domain;
using PaymentSchduler.Domain.Interface;

namespace PaymentSchduler.Controllers
{
    public class HomeController : Controller
    {
        
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

            if (!paymentSchedule.IsValid)
            {
                ViewBag.Message = "The Deposit does not reach the minimum required.";
                return View("Index");
            }

            IPaymentPlanGenerator generator = new PaymentPlanGenerator(paymentSchedule);
            viewModel.PaymentDates = generator.GeneratePlan();
            PopulateNonRequiredFields(viewModel, paymentSchedule);

            return View("Index", viewModel);
        }

        private void PopulateNonRequiredFields(PaymentScheduleViewModel viewModel, PaymentSchedule paymentSchedule)
        {
            viewModel.FirstMonthArrangementFee = PopulateFieldIfRequired(viewModel.FirstMonthArrangementFee, paymentSchedule.FirstMonthArrangementFee);

            viewModel.FinalMonthArrangementFee = PopulateFieldIfRequired(viewModel.FinalMonthArrangementFee, paymentSchedule.FinalMonthArrangementFee);

            viewModel.DepositPercentage = PopulateFieldIfRequired(viewModel.DepositPercentage, paymentSchedule.DepositPercentage);      
        }

        private decimal PopulateFieldIfRequired(decimal? emptyField, decimal populatedField)
        {
            return emptyField == null ? populatedField : (decimal)emptyField;
        }
     
    }
}