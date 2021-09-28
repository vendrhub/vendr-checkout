﻿namespace Vendr.Checkout.Web.Dtos
{
    public class VendrUpdateOrderInformationDto
    {
        public string Email { get; set; }

        public bool MarketingOptIn { get; set; }

        public string CompanyName { get; set; }

        public string TaxCode { get; set; }

        public VendrOrderAddressDto BillingAddress { get; set; }

        public VendrOrderAddressDto ShippingAddress { get; set; }

        public bool ShippingSameAsBilling { get; set; }

        public string Comments { get; set; }

        public int? NextStep { get; set; }
    }
}
