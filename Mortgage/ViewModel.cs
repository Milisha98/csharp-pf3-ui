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

        private List<PIItem> _principleInterestResults;
        public List<PIItem> PrincipleInterestResults 
        { get => _principleInterestResults ?? new List<PIItem>(); 
          set
          {
              Console.WriteLine("VM.PrincipleInterestResults.Set");
              _principleInterestResults = value;

              Console.WriteLine("OneOffPayments:" + OneOffPayments.Count());
          } 
        }

        public List<FixedDebitMutator> OneOffPayments
        {
            get
            {
                // Add One-Off Payments
                var debits = _principleInterestResults.Where(x => x.ExtraPayment.ToDecimal() != 0m)
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
            
        }

        private void Calculate()
        {
            if (!AllowCalculation) return;

            var balanceAmount = (float)model.Balance;
            var repaymentAmount = (float)model.ActualRepayment;
            var interestRate = (float)model.Interest / 100f;

            ITime period = MapPeriod();

            // Patterns
            var interest = new PercentCreditMutator("Interest", AccountName, new DayPeriod(), DateTime.Today, DateTime.MaxValue, (interestRate / 365f), 10, TransactionType.Interest);
            var repayment = new FixedDebitMutator("Repayment", AccountName, period, DateTime.Today, DateTime.MaxValue, repaymentAmount, 5, TransactionType.Payment);
            var mutators = new List<IMutator> { interest, repayment };

            // TODO: If there are one-off payments, then create two scenarios
            // Add any one-off payments
            // if (OneOffPayments != null && !OneOffPayments.Any()) mutators.AddRange(OneOffPayments);

            // Construct Entities
            var pattern = new PublishPattern(new MonthPeriod(), DateTime.Now);
            var ep = new DebitEndPoint(AccountName, 0f);
            var account = new Account(AccountName, AccountType.Credit, 0f, balanceAmount);
            var scenario = new Scenario("Amortisation Schedule", new List<Account> { account }, mutators, new List<IEndPoint> { ep }, pattern);

            // Execute
            var results = scenario.Execute().ToList();
            model.EndDate = results.LastOrDefault()?.When;

            // Publish the Results
            var balancePublisher = new PF3.Publishing.PublishBalance();
            var piPublisher = new PF3.Publishing.PublishPrincipleVsInterest();

            // Display in the Output
            BalanceResults = balancePublisher.CreateColumns(results, PF3.Enums.Period.Year).First();            

            _principleInterestResults = piPublisher.CreateColumns(results, PF3.Enums.Period.Month)
                                                   .Pivot()
                                                   .ToPiItems();

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