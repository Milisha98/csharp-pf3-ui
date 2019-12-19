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
        const string AccountName = "Mortgage";
        private Model model = new Model();          

        public string Balance
        {
            get => model.Balance.ToString("c");
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
                if (AllowCalculation) CalculateOriginal();
            }
        }

        public string EndDate => model.OriginalEndDate?.ToString("dddd, d MMMM yyyy") ?? "";
        public string EndDateInPlainEnglish => model.OriginalEndDate.HasValue ? PF3.Helper.DifferenceInPlainEnglish(DateTime.Now, model.OriginalEndDate.Value) : "";        

        public string OriginalTotalInterest => model.OriginalTotalInterest.ToString("c");
        public string NewTotalInterest => model.NewTotalInterest.ToString("c");
        public string TotalInterestDiff => (model.OriginalTotalInterest - model.NewTotalInterest).ToString("c");
        public bool ShowNewInterest => Model.NewTotalInterest > 0.0;

        public Model Model => model;



        // 
        // User Interface
        //

        public string RepaymentWarningClass => IsRepaymentError ? "danger" : "success";

        public Pane Pane { get; set; }

        public IList<PublishMessage> BalanceResults { get; private set; }

        private List<PIItem> _principleInterestResults;
        public List<PIItem> PrincipleInterestResults 
        { get => _principleInterestResults ?? new List<PIItem>(); 
          set => _principleInterestResults = value;
        }

        public List<FixedDebitMutator> OneOffPayments
        {
            get
            {
                // Add One-Off Payments
                var debits = PrincipleInterestResults.Where(x => x.ExtraPayment.ToDecimal() != 0m)
                                .Select(x => new FixedDebitMutator("One-Off", AccountName, new OneOffPeriod(),x.When,x.When,(float)x.ExtraPayment.ToDecimal(), 10,TransactionType.Contribution))
                                .ToList();

                return debits;             
            }
        }


        //
        // Methods
        //
        
        public void UpdatePrincipleInterestResult(PIItem item)
        {
           
            // Update the Principle Interest Results
            var results = new  List<PIItem>();
            foreach (var result in PrincipleInterestResults)
            {
                if (result.When == item.When)
                    results.Add(item);
                else
                    results.Add(result);
            }
            PrincipleInterestResults = results;
            
            CalculateNew();
        }

        private void CalculateOriginal()
        {
            if (!AllowCalculation) return;

            // Execute
            var results = OriginalScenario.Execute().ToList();
            model.OriginalEndDate = results.LastOrDefault()?.When;
            model.OriginalTotalInterest = 0.0F;
            if (results.Any()) 
                model.OriginalTotalInterest = System.Math.Abs(results.Where(x => x.PublishType == PublishType.LastInterest).Sum(x => x.Balance));

            // Publish the Results
            var balancePublisher = new PF3.Publishing.PublishBalance();
            var piPublisher = new PF3.Publishing.PublishPrincipleVsInterest();

            // Display in the Output
            var output = balancePublisher.CreateColumns(results, PF3.Enums.Period.Year).First();
            BalanceResults = output.ToList();
          

            _principleInterestResults = piPublisher.CreateColumns(results, PF3.Enums.Period.Month)
                                                   .Pivot()
                                                   .ToPiItems();

        }

        private void CalculateNew()
        {
            // Execute
            var scenario = NewScenario;
            if (scenario == null)
            {
                model.NewEndDate = null;
                model.NewTotalInterest = 0;
                return;
            }

            var results = scenario.Execute().ToList();
            model.NewEndDate = results.LastOrDefault()?.When;
            model.NewTotalInterest = 0F;
            if (results.Any()) 
                model.NewTotalInterest = System.Math.Abs(results.Where(x => x.PublishType == PublishType.LastInterest).Sum(x => x.Balance));

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


        private Scenario OriginalScenario
        {
            get
            {
                var balanceAmount = (float)model.Balance;
                var repaymentAmount = (float)model.ActualRepayment;
                var interestRate = (float)model.Interest / 100f;

                ITime period = MapPeriod();

                // Patterns
                var interest = new PercentCreditMutator("Interest", AccountName, new DayPeriod(), DateTime.Today, DateTime.MaxValue, (interestRate / 365f), 10, TransactionType.Interest);
                var repayment = new FixedDebitMutator("Repayment", AccountName, period, DateTime.Today, DateTime.MaxValue, repaymentAmount, 5, TransactionType.Payment);
                var mutators = new List<IMutator> { interest, repayment };

                // Construct Entities
                var pattern = new PublishPattern(new MonthPeriod(), DateTime.Now);
                var ep = new DebitEndPoint(AccountName, 0f);
                var account = new Account(AccountName, AccountType.Credit, 0f, balanceAmount);
                var scenario = new Scenario("Amortisation Schedule", new List<Account> { account }, mutators, new List<IEndPoint> { ep }, pattern);

                return scenario;
            }
        }

        private Scenario NewScenario
        {
            get
            {
                if (!OneOffPayments.Any()) return null;

                var s = OriginalScenario;
                
                // Add any one-off payments
                var mutators = s.Mutators.ToList();
                mutators.AddRange(OneOffPayments);
                s.Mutators = mutators;

                return s;
            }
        }

    }

    public enum Pane
    {
        Balance,
        PrincipleInterest,
        Chart

    }
}