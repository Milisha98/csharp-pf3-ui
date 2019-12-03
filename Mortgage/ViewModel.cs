using System.Collections.Generic;
using PF3.Models;

namespace PF3_UI.Mortgage
{
    public class ViewModel
    {
        private Model model = new Model();          

        public string Balance
        {
            get => model.Balance.ToString("c0");
            set => model.Balance = value.ToDecimal();
        }

        public string Interest
        {
            get => $"{model.Interest.ToString("0.00")}%";
            set => model.Interest = value.ToDecimal();
        }

        public int Term
        {
            get => model.Term;
            set => model.Term = value;
        }

        public int PaymentPeriod
        {
            get => (int)model.PaymentPeriod;
            set => model.PaymentPeriod = (Period)value;
        }

        public decimal? RequiredRepayment
        {
            get
            {
                bool canCalculate = (model.Balance > 0m && model.Interest > 0m);
                if (!canCalculate) return null;
            
                return PMT(model.Interest, 
                            (int)model.PaymentPeriod, 
                            model.Term, 
                            model.Balance);
            }
        }

        public string ActualRepayment
        {
            get => model.ActualRepayment.ToString("c0");
            set => model.ActualRepayment = value.ToDecimal();
        }

        public IEnumerable<PublishMessage> PublishMessages { get; set; }

        private bool IsRepaymentError => 
            model.ActualRepayment > 0m && 
            (this.RequiredRepayment ?? 0m) > model.ActualRepayment;

        public string RepaymentWarningClass => 
            IsRepaymentError ? "danger" : "warning";


        public Model Model => model;

        //
        // Helpers
        //

        private static decimal PMT(decimal yearlyInterestRate, 
                                   int periodsPerYear, 
                                   int years, 
                                   decimal loanAmount)
        {
            double totalPeriods = periodsPerYear * years;
            var rate = (double) yearlyInterestRate / 100 / periodsPerYear;
            var denominator = System.Math.Pow((1 + rate), totalPeriods) - 1;
            return (decimal)(rate + (rate/denominator)) * loanAmount;
        }

    }
}