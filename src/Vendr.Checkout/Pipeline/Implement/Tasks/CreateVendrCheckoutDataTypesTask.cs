using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.Composing;

namespace Vendr.Checkout.Pipeline.Implement.Tasks
{
    internal class CreateVendrCheckoutDataTypesTask : IPipelineTask<InstallPipelineContext, InstallPipelineContext>
    {
        public InstallPipelineContext Process(InstallPipelineContext ctx)
        {
            // Theme Color Picker
            var currentColorPicker = Current.Services.DataTypeService.GetDataType(VendrCheckoutConstants.DataTypes.Guids.ThemeColorPickerGuid);
            if (currentColorPicker == null)
            {
                if (Current.PropertyEditors.TryGet(Constants.PropertyEditors.Aliases.ColorPicker, out IDataEditor editor))
                {
                    var dataType = new DataType(editor)
                    {
                        Key = VendrCheckoutConstants.DataTypes.Guids.ThemeColorPickerGuid,
                        Name = "[Vendr Checkout] Theme Color Picker",
                        DatabaseType = ValueStorageType.Nvarchar,
                        Configuration = new ColorPickerConfiguration
                        {
                            Items = VendrCheckoutConstants.ColorMap.Select((kvp, idx) => new ValueListConfiguration.ValueListItem
                            {
                                Id = idx,
                                Value = "{\"value\":\"" + kvp.Key + "\", \"label\":\"" + kvp.Value + "\"}"
                            }).ToList(),
                            UseLabel = false
                        }
                    };

                    Current.Services.DataTypeService.Save(dataType);
                }
            }
            else
            {
                currentColorPicker.Configuration = new ColorPickerConfiguration
                {
                    Items = VendrCheckoutConstants.ColorMap.Select((kvp, idx) => new ValueListConfiguration.ValueListItem
                    {
                        Id = idx,
                        Value = "{\"value\":\"" + kvp.Key + "\", \"label\":\"" + kvp.Value + "\"}"
                    }).ToList(),
                    UseLabel = false
                };

                Current.Services.DataTypeService.Save(currentColorPicker);
            }

            // Step Picker
            var stepPickerItems = new List<ValueListConfiguration.ValueListItem>
            {
                new ValueListConfiguration.ValueListItem { Id = 1, Value = "Information" },
                new ValueListConfiguration.ValueListItem { Id = 2, Value = "ShippingMethod" },
                new ValueListConfiguration.ValueListItem { Id = 3, Value = "PaymentMethod" },
                new ValueListConfiguration.ValueListItem { Id = 4, Value = "Review" },
                new ValueListConfiguration.ValueListItem { Id = 5, Value = "Payment" },
                new ValueListConfiguration.ValueListItem { Id = 6, Value = "Confirmation" }
            };

            var currentStepPicker = Current.Services.DataTypeService.GetDataType(VendrCheckoutConstants.DataTypes.Guids.StepPickerGuid);
            if (currentStepPicker == null)
            {
                if (Current.PropertyEditors.TryGet(Constants.PropertyEditors.Aliases.DropDownListFlexible, out IDataEditor editor))
                {
                    var dataType = new DataType(editor)
                    {
                        Key = VendrCheckoutConstants.DataTypes.Guids.StepPickerGuid,
                        Name = "[Vendr Checkout] Step Picker",
                        DatabaseType = ValueStorageType.Nvarchar,
                        Configuration = new DropDownFlexibleConfiguration
                        {
                            Items = stepPickerItems,
                            Multiple = false
                        }
                    };

                    Current.Services.DataTypeService.Save(dataType);
                }
            }
            else
            {
                currentStepPicker.Configuration = new DropDownFlexibleConfiguration
                {
                    Items = stepPickerItems,
                    Multiple = false
                };

                Current.Services.DataTypeService.Save(currentStepPicker);
            }

            // Continue the pipeline
            return ctx;
        }
    }
}