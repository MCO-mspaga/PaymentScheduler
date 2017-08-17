using PaymentSchduler.Models;
using PaymentSchduler.ViewModels;

namespace PaymentSchduler.Domain.Interface
{
    public interface ILoanGenerator
    {
        PaymentScheduleViewModel GenerateLoan(PaymentScheduleViewModel viewModel);
    }
}
