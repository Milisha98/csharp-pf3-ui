using System;

namespace PF3_UI.Mortgage
{
    public class PIItem
    {
        public DateTime When { get; set; }

        public float Principle { get; set; }

        public float Interest { get; set; }

        private decimal? _editPrinciple;
        public string EditPrinciple
        {
            get => (_editPrinciple ?? (decimal)Principle).ToString("c0");
            set
            {
                _editPrinciple = value.ToDecimal();
            }
        }

        public string BackgroundColor
        {
            get
            {
                if (Principle > Interest) return "bg-success";
                if (Principle < Interest) return "bg-danger";
                return "bg-light";
            }
        }

    }
}