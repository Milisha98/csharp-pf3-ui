using PF3_UI.Model;

namespace PF3_UI.Mortgage.Events
{
    public class SetBalance : IUpdateEvent
    {
        public string Value { get; set; }
    }

}