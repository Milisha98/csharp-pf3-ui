using System;

namespace PF3_UI.Mortgage
{
    public class Model
    {
        public decimal Balance { get; set; }
        public decimal Interest { get; set; } = 4.75m;
        public int Term { get; set; } = 25;
        public decimal ActualRepayment { get; set; }
        public Period PaymentPeriod { get; set; } = Period.Monthly;
        public DateTime? EndDate { get; set; }
    }

    public enum Period
    {
        Monthly = 12,
        Fortnightly = 26,
        Weekly = 52,
    }
}