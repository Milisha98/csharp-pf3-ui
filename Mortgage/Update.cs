namespace PF3_UI.Mortgage
{
    public static class Update
    {
        public static Model UpdateBalance(this Model model, string value)
        {
            model.Balance = value.ToDecimal();
            model.UpdateRequiredRepayment();
            return model;
        }
        
        public static Model UpdateInterest(this Model model, string value)
        {
            model.Interest = value.ToDecimal();
            model.UpdateRequiredRepayment();
            return model;
        }

        public static Model UpdateYears(this Model model, int value)
        {
            model.Years = value;
            model.UpdateRequiredRepayment();
            return model;
        }

        public static Model UpdatePeriod(this Model model, int value)
        {
            model.Period = (Period)value;
            model.UpdateRequiredRepayment();
            return model;
        }        

        public static Model UpdateActualRepayment(this Model model, string value)
        {
            model.ActualRepayment = value.ToDecimal();
            return model;
        }

        private static void UpdateRequiredRepayment(this Model model)
        {
            bool canCalculate = (model.Balance > 0m && model.Interest > 0m);
            
            if (!canCalculate) return;
            model.RequiredRepayment = PMT(model.Interest, (int)model.Period, model.Years, model.Balance);
        }

        //
        // Helpers
        //
        private static decimal PMT(decimal yearlyInterestRate, int periodsPerYear, int years, decimal loanAmount)
        {
            double totalPeriods = periodsPerYear * years;
            var rate = (double) yearlyInterestRate / 100 / periodsPerYear;
            var denominator = System.Math.Pow((1 + rate), totalPeriods) - 1;
            return (decimal)(rate + (rate/denominator)) * loanAmount;
        }
    }
}