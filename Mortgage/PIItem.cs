using System;

namespace PF3_UI.Mortgage
{
    public class PIItem
    {
        public DateTime When { get; set; }

        public float Principle { get; set; }

        public float Interest { get; set; }

        public string BackgroundColor
        {
            get
            {
                if (Total > (decimal)Interest) return "bg-success";
                if (Total < (decimal)Interest) return "bg-danger";
                return "bg-light";
            }
        }

        private Decimal _extraPayment = 0m;
        public string ExtraPayment 
        { 
            get => _extraPayment.ToString("c0");
            set
            {
                _extraPayment = value.ToDecimal();
            }
        }

        public decimal Total => (decimal)Principle + _extraPayment;
    }
}