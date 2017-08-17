using System.Web.Mvc;
using PaymentSchduler.Models;
using PaymentSchduler.ViewModels;
using PaymentSchduler.Domain;
using PaymentSchduler.Domain.Interface;

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
            ILoanGenerator generator = new LoanGenerator();
            

            viewModel = generator.GenerateLoan(viewModel);
            
                       
            return View("Index", viewModel);
        }
     
    }
}