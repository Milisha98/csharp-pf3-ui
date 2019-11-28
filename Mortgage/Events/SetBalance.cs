using Microsoft.AspNetCore.Components;
using PF3_UI.Model;

namespace PF3_UI.Mortgage.Events
{
    public class SetBalance : IUpdateEvent
    {
        public SetBalance(ChangeEventArgs e)
        {
            Value = e.Value.ToString();
        }
        public string Value { get; set; }
    }

}