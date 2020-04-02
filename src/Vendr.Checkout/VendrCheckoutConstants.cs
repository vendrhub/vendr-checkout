using System;
using System.Collections.Generic;

namespace Vendr.Checkout
{
    public static class VendrCheckoutConstants
    {
        public static IDictionary<string, string> ColorMap = new Dictionary<string, string>
        {
            { "000000", "black" },
            { "f56565", "red-500" },
            { "ed8936", "orange-500" },
            { "ecc94b", "yellow-500" },
            { "48bb78", "green-500" },
            { "38b2ac", "teal-500" },
            { "4299e1", "blue-500" },
            { "667eea", "indigo-500" },
            { "9f7aea", "purple-500" },
            { "ed64a6", "pink-500" }
        };

        public static class DataTypes
        {
            public static class Guids
            {
                public const string ThemeColorPicker = "46322397-3b7b-4d53-a5db-a1b17553d397";
                public static readonly Guid ThemeColorPickerGuid = new Guid(ThemeColorPicker);

                public const string StepPicker = "654a2147-2559-4b3f-93ee-a6925f45c173";
                public static readonly Guid StepPickerGuid = new Guid(StepPicker);
            }
        }

        public static class ContentTypes
        {
            public static class Aliases
            {
                public const string BasePage = "vendrCheckoutBasePage";
                public const string CheckoutPage = "vendrCheckoutCheckoutPage";
                public const string CheckoutStepPage = "vendrCheckoutCheckoutStepPage";
            }

            public static class Guids
            {
                public const string BasePage = "55f4b88e-69b6-45a4-bc4f-d48f35f6b904";
                public static readonly Guid BasePageGuid = new Guid(BasePage);

                public const string CheckoutPage = "e5e809cf-f3e5-4bb8-b7bb-c67c8303c2f4";
                public static readonly Guid CheckoutPageGuid = new Guid(CheckoutPage);

                public const string CheckoutStepPage = "d9384576-e6a8-4ef2-8ca8-475ee6e7546d";
                public static readonly Guid CheckoutStepPageGuid = new Guid(CheckoutStepPage);
            }
        }
    }
}