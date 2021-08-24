using System;

namespace Vendr.Checkout.Web.Dtos
{
    public class VendrUpdateOrderPaymentMethodDto
    {
        public Guid PaymentMethod { get; set; }

        public Guid? NextStep { get; set; }
    }
}
