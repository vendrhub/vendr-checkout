using System;

namespace Vendr.Checkout.Web.Dtos
{
    public class VendrUpdateOrderShippingAndPaymentMethodDto
    {
        public Guid ShippingMethod { get; set; }

        public Guid PaymentMethod { get; set; }

        public int? NextStep { get; set; }
    }
}
