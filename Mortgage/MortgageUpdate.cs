
using PF3_UI.Model;
using PF3_UI.Mortgage.Events;

namespace PF3_UI.Mortgage
{
    public static class MortgageUpdate
    {
        public static MortgageUI Update(MortgageUI model, IUpdateEvent msg)
        {
            switch (msg)
            {
                case Events.SetBalance e:
                    model.Balance = e.Value.FormatAsCurrency();
                    break;

                default:
                    break;
            }

            return model;

        }

    }
}