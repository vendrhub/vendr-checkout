using System;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web.Composing;

namespace Vendr.Checkout.Pipeline.Implement.Tasks
{
    internal class CreateVendrCheckoutDocumentTypesTask : IPipelineTask<InstallPipelineContext, InstallPipelineContext>
    {
        public InstallPipelineContext Process(InstallPipelineContext ctx)
        {
            IContentType existing;
            int checkoutPageContentTypeId;
            int checkoutStepPageContentTypeId;

            // Setup lazy data types
            var textstringDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(Constants.DataTypes.Guids.TextstringGuid));
            var textareaDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(Constants.DataTypes.Guids.TextareaGuid));
            var booleanDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(Constants.DataTypes.Guids.CheckboxGuid));
            var contentPickerDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(Constants.DataTypes.Guids.ContentPickerGuid));
            var mediaPickerDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(Constants.DataTypes.Guids.MediaPickerGuid));
            var themeColorPickerDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(VendrCheckoutConstants.DataTypes.Guids.ThemeColorPickerGuid));
            var stepPickerDataType = new Lazy<IDataType>(() => Current.Services.DataTypeService.GetDataType(VendrCheckoutConstants.DataTypes.Guids.StepPickerGuid));

            // Checkout Base Page
            existing = Current.Services.ContentTypeService.Get(VendrCheckoutConstants.ContentTypes.Guids.BasePageGuid);
            if (existing == null)
            {

                var contentType = new ContentType(-1)
                {
                    Key = VendrCheckoutConstants.ContentTypes.Guids.BasePageGuid,
                    Alias = "vendrCheckoutBasePage",
                    Name = "[Vendr Checkout] Page"
                };

                Current.Services.ContentTypeService.Save(contentType);

                checkoutPageContentTypeId = contentType.Id;
            }
            else
            {
                checkoutPageContentTypeId = existing.Id;
            }

            // Checkout Step Page
            existing = Current.Services.ContentTypeService.Get(VendrCheckoutConstants.ContentTypes.Guids.CheckoutStepPageGuid);
            if (existing == null)
            {
                var contentType = new ContentType(checkoutPageContentTypeId)
                {
                    Key = VendrCheckoutConstants.ContentTypes.Guids.CheckoutStepPageGuid,
                    Alias = "vendrCheckoutCheckoutStepPage",
                    Name = "[Vendr Checkout] Checkout Step Page",
                    PropertyGroups = new PropertyGroupCollection(new[]{
                        new PropertyGroup(new PropertyTypeCollection(true, new[]{
                            new PropertyType(textstringDataType.Value) {
                                Alias = "vendrShortStepName",
                                Name = "Short Step Name",
                                Description = "A short name for this step to display in the checkout navigation.",
                                SortOrder = 10
                            },
                            new PropertyType(stepPickerDataType.Value) {
                                Alias = "vendrStepType",
                                Name = "Step Type",
                                Description = "The checkout step to display for this step of the checkout flow.",
                                SortOrder = 20
                            }
                        })) {
                            Name = "Settings",
                            SortOrder =100
                        }
                    })
                };

                Current.Services.ContentTypeService.Save(contentType);

                checkoutStepPageContentTypeId = contentType.Id;
            }
            else
            {
                checkoutStepPageContentTypeId = existing.Id;
            }

            // Checkout Page
            if (Current.Services.ContentTypeService.Get(VendrCheckoutConstants.ContentTypes.Guids.CheckoutPageGuid) == null)
            {
                var contentType = new ContentType(checkoutPageContentTypeId)
                {
                    Key = VendrCheckoutConstants.ContentTypes.Guids.CheckoutPageGuid,
                    Alias = "vendrCheckoutCheckoutPage",
                    Name = "[Vendr Checkout] Checkout Page",
                    AllowedContentTypes = new[]{
                        new ContentTypeSort(checkoutStepPageContentTypeId, 1)
                    },
                    PropertyGroups = new PropertyGroupCollection(new[]{
                        new PropertyGroup(new PropertyTypeCollection(true, new[]{
                            new PropertyType(mediaPickerDataType.Value) {
                                Alias = "vendrStoreLogo",
                                Name = "Store Logo",
                                Description = "A logo image for the store to appear at the top of the checkout screens and order emails.",
                                SortOrder = 10
                            },
                            new PropertyType(textstringDataType.Value) {
                                Alias = "vendrStoreAddress",
                                Name = "Store Address",
                                Description = "The address of the web store to appear in the footer of order emails.",
                                SortOrder = 20
                            },
                            new PropertyType(themeColorPickerDataType.Value) {
                                Alias = "vendrThemeColor",
                                Name = "Theme Color",
                                Description = "The theme color to use for colored elements of the checkout pages.",
                                SortOrder = 30
                            },
                            new PropertyType(booleanDataType.Value) {
                                Alias = "vendrCollectShippingDetails",
                                Name = "Collect Shipping Details",
                                Description = "Select whether to collect shipping details or not. Not necessary if you are only dealing with digital downloads.",
                                SortOrder = 40
                            },
                            new PropertyType(textstringDataType.Value) {
                                Alias = "vendrOrderLinePropertyAliases",
                                Name = "Order Line Property Aliases",
                                Description = "Comma separated list of order line property aliases to display in the order summary.",
                                SortOrder = 50
                            },
                            new PropertyType(textstringDataType.Value) {
                                Alias = "vendrCheckoutBackPage",
                                Name = "Checkout Back Page",
                                Description = "The page to go back to when backing out of the checkout flow.",
                                SortOrder = 60
                            },
                            new PropertyType(contentPickerDataType.Value) {
                                Alias = "vendrTermsAndConditionsPage",
                                Name = "Terms and Conditions Page",
                                Description = "The page on the site containing the terms and conditions.",
                                SortOrder = 70
                            },
                            new PropertyType(contentPickerDataType.Value) {
                                Alias = "vendrPrivacyPolicyPage",
                                Name = "Privacy Policy Page",
                                Description = "The page on the site containing the privacy policy.",
                                SortOrder = 80
                            },
                            new PropertyType(contentPickerDataType.Value) {
                                Alias = "umbracoNaviHide",
                                Name = "Hide from Navigation",
                                Description = "Hide the checkout page from the sites main navigation.",
                                SortOrder = 90
                            }
                        })) {
                            Name = "Settings",
                            SortOrder = 50
                        }
                    })
                };

                Current.Services.ContentTypeService.Save(contentType);
            }

            // Continue the pipeline
            return ctx;
        }
    }
}