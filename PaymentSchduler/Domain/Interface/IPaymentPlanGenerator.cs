using PaymentSchduler.Models;
using PaymentSchduler.ViewModels;
using System.Collections.Generic;

namespace PaymentSchduler.Domain.Interface
{
    public interface IPaymentPlanGenerator
    {
        List<PaymentAndDate>  GeneratePlan();
    }
}
