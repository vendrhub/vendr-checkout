using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Vendr.Checkout.Composing
{
    public class VendrCheckoutComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<VendrCheckoutComponent>();
        }
    }
}