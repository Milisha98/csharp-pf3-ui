namespace PF3_UI.Mortgage
{
    public class Model
    {
        public decimal Balance { get; set; }
        public decimal Interest { get; set; } = 4.75m;
        public int Years { get; set; } = 25;
        public decimal RequiredRepayment { get; set; }
        public decimal ActualRepayment { get; set; }
        public Period Period { get; set; } = Period.Monthly;
    }

    public enum Period
    {
        Monthly = 12,
        Fortnightly = 26,
        Weekly = 52,
    }
}