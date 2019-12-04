using System;
using System.Collections.Generic;
using System.Linq;
using PF3.EndPoints;
using PF3.Enums;
using PF3.Models;
using PF3.Mutators;
using PF3.Time;

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
            set 
            {
                model.ActualRepayment = value.ToDecimal();
                if (AllowCalculation) Calculate();
            }
        }

        public string EndDate => model.EndDate?.ToString("dddd, d MMMM yyyy") ?? "";
        public string EndDateInPlainEnglish => model.EndDate.HasValue ? PF3.Helper.DifferenceInPlainEnglish(DateTime.Now, model.EndDate.Value) : "";        
        public Model Model => model;


        // 
        // User Interface
        //

        public string RepaymentWarningClass => IsRepaymentError ? "danger" : "success";


        public bool ShowBalancePanel { get; set; } = true;        


        public IEnumerable<PublishMessage> BalanceResults { get; private set; }



        //
        // Methods
        //
        
        public void Calculate()
        {
            if (!AllowCalculation) return;

            var balanceAmount = (float)model.Balance;
            var repaymentAmount = (float)model.ActualRepayment;
            var interestRate = (float)model.Interest / 100f;
            var accountName = "Mortgage";

            ITime period = MapPeriod();

            // Patterns
            var interest = new PercentCreditMutator("Interest", accountName, new DayPeriod(), DateTime.Today, DateTime.MaxValue, (interestRate / 365f), 10, TransactionType.Interest);
            var repayment = new FixedDebitMutator("Repayment", accountName, period, DateTime.Today, DateTime.MaxValue, repaymentAmount, 5, TransactionType.Payment);
            var mutators = new List<IMutator> { interest, repayment };

            // Construct Entities
            var pattern = new PublishPattern(new MonthPeriod(), DateTime.Now);
            var ep = new DebitEndPoint(accountName, 0f);
            var account = new Account(accountName, AccountType.Credit, 0f, balanceAmount);
            var scenario = new Scenario("Amortisation Schedule", new List<Account> { account }, mutators, new List<IEndPoint> { ep }, pattern);

            // Execute
            var results = scenario.Execute().ToList();
            model.EndDate = results.LastOrDefault()?.When;

            // Publish the Results
            var balancePublisher = new PF3.Publishing.PublishBalance();
            var piPublisher = new PF3.Publishing.PublishPrincipleVsInterest();

            BalanceResults = balancePublisher.CreateColumns(results, PF3.Enums.Period.Year).First();

        }


        //
        // Helpers
        //

        private bool AllowCalculation => (model.Balance > 0m) && (model.ActualRepayment > 0m) && !IsRepaymentError;

        private bool IsRepaymentError => 
            model.ActualRepayment > 0m && 
            (this.RequiredRepayment ?? 0m) > model.ActualRepayment;


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

        private ITime MapPeriod()
        {
            ITime period;
            switch (model.PaymentPeriod)
            {
                case Mortgage.Period.Weekly:
                    period = new WeekPeriod();
                    break;
                case Mortgage.Period.Fortnightly:
                    period = new WeekPeriod(2);
                    break;
                default:
                    period = new MonthPeriod();
                    break;
            }

            return period;
        }        

    }
}