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
            // Setup variables
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
                    Alias = VendrCheckoutConstants.ContentTypes.Aliases.BasePage,
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
            var checkoutStepProps = new[]{
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
            };

            existing = Current.Services.ContentTypeService.Get(VendrCheckoutConstants.ContentTypes.Guids.CheckoutStepPageGuid);
            
            if (existing == null)
            {
                var contentType = new ContentType(checkoutPageContentTypeId)
                {
                    Key = VendrCheckoutConstants.ContentTypes.Guids.CheckoutStepPageGuid,
                    Alias = VendrCheckoutConstants.ContentTypes.Aliases.CheckoutStepPage,
                    Name = "[Vendr Checkout] Checkout Step Page",
                    Icon = "icon-settings-alt color-green",
                    PropertyGroups = new PropertyGroupCollection(new[]{
                        new PropertyGroup(new PropertyTypeCollection(true, checkoutStepProps)) {
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
                var safeExisting = false;
                var hasSettingsGroup = existing.PropertyGroups.Contains("settings");
                var settingsGroup = hasSettingsGroup
                    ? existing.PropertyGroups["Settings"]
                    : new PropertyGroup(new PropertyTypeCollection(true, checkoutStepProps)) 
                    {
                        Name = "Settings",
                        SortOrder =100
                    };

                foreach (var prop in checkoutStepProps)
                {
                    if (!settingsGroup.PropertyTypes.Contains(prop.Alias))
                    {
                        settingsGroup.PropertyTypes.Add(prop);
                        safeExisting = true;
                    }
                }

                if (!hasSettingsGroup)
                {
                    existing.PropertyGroups.Add(settingsGroup);
                    safeExisting = true;
                }

                if (safeExisting)
                {
                    Current.Services.ContentTypeService.Save(existing);
                }

                checkoutStepPageContentTypeId = existing.Id;
            }

            // Checkout Page
            var checkoutPageProps = new[]{
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
                    Alias = "vendrCollectShippingInfo",
                    Name = "Collect Shipping Info",
                    Description = "Select whether to collect shipping information or not. Not necessary if you are only dealing with digital downloads.",
                    SortOrder = 40
                },
                new PropertyType(textstringDataType.Value) {
                    Alias = "vendrOrderLinePropertyAliases",
                    Name = "Order Line Property Aliases",
                    Description = "Comma separated list of order line property aliases to display in the order summary.",
                    SortOrder = 50
                },
                new PropertyType(contentPickerDataType.Value) {
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
                new PropertyType(booleanDataType.Value) {
                    Alias = "umbracoNaviHide",
                    Name = "Hide from Navigation",
                    Description = "Hide the checkout page from the sites main navigation.",
                    SortOrder = 90
                }
            };

            existing = Current.Services.ContentTypeService.Get(VendrCheckoutConstants.ContentTypes.Guids.CheckoutPageGuid);

            if (existing == null)
            {
                var contentType = new ContentType(checkoutPageContentTypeId)
                {
                    Key = VendrCheckoutConstants.ContentTypes.Guids.CheckoutPageGuid,
                    Alias = VendrCheckoutConstants.ContentTypes.Aliases.CheckoutPage,
                    Name = "[Vendr Checkout] Checkout Page",
                    Icon = "icon-cash-register color-green",
                    AllowedContentTypes = new[]{
                        new ContentTypeSort(checkoutStepPageContentTypeId, 1)
                    },
                    PropertyGroups = new PropertyGroupCollection(new[]{
                        new PropertyGroup(new PropertyTypeCollection(true, checkoutPageProps)) {
                            Name = "Settings",
                            SortOrder = 50
                        }
                    })
                };

                Current.Services.ContentTypeService.Save(contentType);
            }
            else
            {
                var safeExisting = false;
                var hasSettingsGroup = existing.PropertyGroups.Contains("settings");
                var settingsGroup = hasSettingsGroup
                    ? existing.PropertyGroups["Settings"]
                    : new PropertyGroup(new PropertyTypeCollection(true, checkoutPageProps))
                    {
                        Name = "Settings",
                        SortOrder = 100
                    };

                foreach (var prop in checkoutPageProps)
                {
                    if (!settingsGroup.PropertyTypes.Contains(prop.Alias))
                    {
                        settingsGroup.PropertyTypes.Add(prop);
                        safeExisting = true;
                    }
                }

                if (!hasSettingsGroup)
                {
                    existing.PropertyGroups.Add(settingsGroup);
                    safeExisting = true;
                }

                if (safeExisting)
                {
                    Current.Services.ContentTypeService.Save(existing);
                }
            }

            // Continue the pipeline
            return ctx;
        }
    }
}