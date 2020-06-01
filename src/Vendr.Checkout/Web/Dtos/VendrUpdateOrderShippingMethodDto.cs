using System;

namespace Vendr.Checkout.Web.Dtos
{
    public class VendrUpdateOrderShippingMethodDto
    {
        public Guid ShippingMethod { get; set; }

        public int? NextStep { get; set; }
    }
}
