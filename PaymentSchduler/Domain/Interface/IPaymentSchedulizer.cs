using PaymentSchduler.Models;
using System.Collections.Generic;

namespace PaymentSchduler.Domain.Interface
{
    public interface IPaymentSchedulizer
    {

        List<PaymentAndDate> PreparePaymentSchedule(PaymentSchedule paymentSchedule);
    }
}
