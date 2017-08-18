using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentSchduler.ViewModels;
using PaymentSchduler.Domain.Interface;
using PaymentSchduler.Domain;
using PaymentSchduler.Models;
using System.Collections.Generic;
using PaymentSchduler.Models;


namespace PaymenScheduler.Tests
{
    [TestClass]
    public class PaymentPlanGeneratorTests
    {
        [TestMethod]
        public void GenerateLoan_CalculatePaymentPlan_SUccessfulCreation()
        {
            #region Arrange
            PaymentScheduleViewModel viewModel = new PaymentScheduleViewModel();
            viewModel.VehiclePrice = 1000;
            viewModel.DepositAmount = 150;
            viewModel.FinanceOption = 1;
            viewModel.DeliveryDate = CreateDate("17/08/2017");
            viewModel.FirstMonthArrangementFee = 88;
            viewModel.FinalMonthArrangementFee = 20;
            viewModel.DepositPercentage = 0.15m;
            PaymentSchedule payment = new PaymentSchedule(viewModel);

            PaymentScheduleViewModel expectedModel = viewModel;

            expectedModel.PaymentDates = new List<PaymentAndDate>
            {
                new PaymentAndDate { PaymentDate = CreateDate("02/10/2017"), PaymentValue = 158.83m },
                new PaymentAndDate { PaymentDate = CreateDate("06/11/2017"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("04/12/2017"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("01/01/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("05/02/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("05/03/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("02/04/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("07/05/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("04/06/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("02/07/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("06/08/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("03/09/2018"), PaymentValue = 90.87m },
            };
            #endregion 
            
            #region Act
            IPaymentPlanGenerator generator = new PaymentPlanGenerator(payment);
            viewModel.PaymentDates = generator.GeneratePlan();
            #endregion 

            #region Assert
            Assert.AreEqual(expectedModel, viewModel);
            #endregion 
        }


        [TestMethod]
        public void GenerateLoan_CalculatePaymentPlanWithNonRequiredFieldEmpty_SuccessfulCreation()
        {
            #region Arrange
            PaymentScheduleViewModel viewModel = new PaymentScheduleViewModel();
            viewModel.VehiclePrice = 1000;
            viewModel.DepositAmount = 150;
            viewModel.FinanceOption = 1;
            viewModel.DeliveryDate = CreateDate("17/08/2017");
            PaymentScheduleViewModel expectedModel = viewModel;
            PaymentSchedule payment = new PaymentSchedule(viewModel);

            expectedModel.PaymentDates = new List<PaymentAndDate>
            {
                new PaymentAndDate { PaymentDate = CreateDate("02/10/2017"), PaymentValue = 158.83m },
                new PaymentAndDate { PaymentDate = CreateDate("06/11/2017"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("04/12/2017"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("01/01/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("05/02/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("05/03/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("02/04/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("07/05/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("04/06/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("02/07/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("06/08/2018"), PaymentValue = 70.83m },
                new PaymentAndDate { PaymentDate = CreateDate("03/09/2018"), PaymentValue = 90.87m },
            };
            #endregion

            #region Act
            IPaymentPlanGenerator generator = new PaymentPlanGenerator(payment);
            viewModel.PaymentDates = generator.GeneratePlan();
            #endregion 

            #region Assert
            Assert.AreEqual(expectedModel, viewModel);
            #endregion 
        }

        [TestMethod]
        public void PaymentSchedule_DepositIsEqualToRequiredMin_IsValid()
        {
            #region arrange
            PaymentScheduleViewModel viewModel = new PaymentScheduleViewModel();
            viewModel.VehiclePrice = 1000;
            viewModel.DepositAmount = 150m;
            viewModel.FinanceOption = 1;
            viewModel.DeliveryDate = CreateDate("17/08/2017");
            viewModel.FirstMonthArrangementFee = 88m;
            viewModel.FinalMonthArrangementFee = 20m;
            viewModel.DepositPercentage = 0.15m;
            bool expected = true;

            #endregion

            #region act

            PaymentSchedule payment = new PaymentSchedule(viewModel);

            #endregion

            #region assert 

            //would have used isTrue but was throwing an error inside the test library. 
            Assert.AreEqual(expected, payment.IsValid);
            
            #endregion

        }


        [TestMethod]
        public void PaymentSchedule_DepositIsEqualToRequiredMin_IsNotValid()
        {
            #region arrange
            PaymentScheduleViewModel viewModel = new PaymentScheduleViewModel();
            viewModel.VehiclePrice = 1000;
            viewModel.DepositAmount = 10m;
            viewModel.FinanceOption = 1;
            viewModel.DeliveryDate = CreateDate("17/08/2017");
            viewModel.FirstMonthArrangementFee = 88m;
            viewModel.FinalMonthArrangementFee = 20m;
            viewModel.DepositPercentage = 0.15m;
            bool expected = false;

            #endregion

            #region act

            PaymentSchedule payment = new PaymentSchedule(viewModel);

            #endregion

            #region assert 
            
            Assert.AreEqual(expected, payment.IsValid);
            
            #endregion

        }



        private DateTime CreateDate(string date)
        {
            return DateTime.ParseExact(date, "dd/MM/yyyy", null);
        }
        
    }
}
