namespace Vendr.Checkout.Configuration
{
    public class VendrCheckoutSettings
    {
        public string RootViewPath { get; set; }

        public bool ResetPaymentMethodOnShippingMethodChange { get; set; }

        public VendrCheckoutSettings()
        {
            RootViewPath = "/App_Plugins/VendrCheckout/views";
        }
    }
}
