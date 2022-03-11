﻿using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Vendr.Checkout.Web.Dtos;
using Vendr.Core;
using Vendr.Core.Web.Api;
using Vendr.Core.Exceptions;
using Umbraco.Core;
using VendrConstants = Vendr.Core.Constants;
using System.Linq;

namespace Vendr.Checkout.Web.Controllers
{
    public class VendrCheckoutSurfaceController : SurfaceController, IRenderController
    {
        private readonly IVendrApi _vendrApi;

        public VendrCheckoutSurfaceController(IVendrApi vendrAPi)
        {
            _vendrApi = vendrAPi;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyDiscountOrGiftCardCode(VendrDiscountOrGiftCardCodeDto model)
        {
            try
            {
                using (var uow = _vendrApi.Uow.Create())
                {
                    var store = CurrentPage.GetStore();
                    var order = _vendrApi.GetCurrentOrder(store.Id)
                        .AsWritable(uow)
                        .Redeem(model.Code);

                    _vendrApi.SaveOrder(order);

                    uow.Complete();
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", string.Format(Umbraco.GetDictionaryValue("VendrCheckout.Information.InvalidDiscountGiftCode", "Failed to redeem discount code: {0}"), ex.Message));

                return IsAjaxRequest()
                    ? (ActionResult)Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) })
                    : CurrentUmbracoPage();
            }

            return IsAjaxRequest()
                ? (ActionResult)Json(new { success = true })
                : RedirectToCurrentUmbracoPage();
        }

        [HttpGet]
        public ActionResult RemoveDiscountOrGiftCardCode(VendrDiscountOrGiftCardCodeDto model)
        {
            try
            {
                using (var uow = _vendrApi.Uow.Create())
                {
                    var store = CurrentPage.GetStore();
                    var order = _vendrApi.GetCurrentOrder(store.Id)
                        .AsWritable(uow)
                        .Unredeem(model.Code);

                    _vendrApi.SaveOrder(order);

                    uow.Complete();
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", "Failed to unredeem discount code: " + ex.Message);

                return IsAjaxRequest()
                    ? (ActionResult)Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) }, JsonRequestBehavior.AllowGet)
                    : CurrentUmbracoPage();
            }

            return IsAjaxRequest()
                ? (ActionResult)Json(new { success = true }, JsonRequestBehavior.AllowGet)
                : RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOrderInformation(VendrUpdateOrderInformationDto model)
        {
            try
            {
                var checkoutPage = CurrentPage.GetCheckoutPage();

                using (var uow = _vendrApi.Uow.Create())
                {
                    var store = CurrentPage.GetStore(); 
                    var order = _vendrApi.GetCurrentOrder(store.Id)
                        .AsWritable(uow)
                        .SetProperties(new Dictionary<string, string>
                        {
                            { VendrConstants.Properties.Customer.EmailPropertyAlias, model.Email },
                            { "marketingOptIn", model.MarketingOptIn ? "1" : "0" },

                            { VendrConstants.Properties.Customer.FirstNamePropertyAlias, model.BillingAddress.FirstName },
                            { VendrConstants.Properties.Customer.LastNamePropertyAlias, model.BillingAddress.LastName },
                            { "billingAddressLine1", model.BillingAddress.Line1 },
                            { "billingAddressLine2", model.BillingAddress.Line2 },
                            { "billingCity", model.BillingAddress.City },
                            { "billingZipCode", model.BillingAddress.ZipCode },
                            { "billingTelephone", model.BillingAddress.Telephone },
                            { "comments", model.Comments },
                            { "ipAddress", GetIPAddress() }
                        })
                        .SetPaymentCountryRegion(model.BillingAddress.Country, model.BillingAddress.Region);

                    if (checkoutPage.Value<bool>("vendrCollectShippingInfo"))
                    {
                        order.SetProperties(new Dictionary<string, string>
                        {
                            { "shippingSameAsBilling", model.ShippingSameAsBilling ? "1" : "0" },
                            { "shippingFirstName", model.ShippingSameAsBilling? model.BillingAddress.FirstName : model.ShippingAddress.FirstName },
                            { "shippingLastName", model.ShippingSameAsBilling? model.BillingAddress.LastName : model.ShippingAddress.LastName },
                            { "shippingAddressLine1", model.ShippingSameAsBilling? model.BillingAddress.Line1 : model.ShippingAddress.Line1 },
                            { "shippingAddressLine2", model.ShippingSameAsBilling? model.BillingAddress.Line2 : model.ShippingAddress.Line2 },
                            { "shippingCity", model.ShippingSameAsBilling? model.BillingAddress.City : model.ShippingAddress.City },
                            { "shippingZipCode", model.ShippingSameAsBilling? model.BillingAddress.ZipCode : model.ShippingAddress.ZipCode },
                            { "shippingTelephone", model.ShippingSameAsBilling? model.BillingAddress.Telephone : model.ShippingAddress.Telephone }
                        })
                        .SetShippingCountryRegion(model.ShippingSameAsBilling ? model.BillingAddress.Country : model.ShippingAddress.Country, 
                            model.ShippingSameAsBilling ? model.BillingAddress.Region : model.ShippingAddress.Region);
                    }
                    else 
                    {
                        order.SetShippingCountryRegion(model.BillingAddress.Country, null)
                            .ClearShippingMethod();
                    }

                    _vendrApi.SaveOrder(order);

                    uow.Complete();
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", "Failed to update information: " + ex.Message);

                return IsAjaxRequest()
                    ? (ActionResult)Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) })
                    : CurrentUmbracoPage();
            }

            return IsAjaxRequest()
                ? (ActionResult)Json(new { success = true })
                : model.NextStep.HasValue
                    ? RedirectToUmbracoPage(model.NextStep.Value)
                    : RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOrderShippingMethod(VendrUpdateOrderShippingMethodDto model)
        {
            try
            {
                using (var uow = _vendrApi.Uow.Create())
                {
                    var checkoutPage = CurrentPage.GetCheckoutPage();
                    var store = CurrentPage.GetStore();
                    var order = _vendrApi.GetCurrentOrder(store.Id)
                        .AsWritable(uow)
                        .SetShippingMethod(model.ShippingMethod);

                    _vendrApi.SaveOrder(order);

                    uow.Complete();
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", "Failed to update shipping method: " + ex.Message);

                return IsAjaxRequest()
                    ? (ActionResult)Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) })
                    : CurrentUmbracoPage();
            }

            return IsAjaxRequest()
                ? (ActionResult)Json(new { success = true })
                : model.NextStep.HasValue
                    ? RedirectToUmbracoPage(model.NextStep.Value)
                    : RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOrderPaymentMethod(VendrUpdateOrderPaymentMethodDto model)
        {
            try
            {
                using (var uow = _vendrApi.Uow.Create())
                {
                    var checkoutPage = CurrentPage.GetCheckoutPage();
                    var store = CurrentPage.GetStore();
                    var order = _vendrApi.GetCurrentOrder(store.Id)
                        .AsWritable(uow)
                        .SetPaymentMethod(model.PaymentMethod);

                    _vendrApi.SaveOrder(order);

                    uow.Complete();
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", "Failed to update payment method: " + ex.Message);

                return IsAjaxRequest()
                    ? (ActionResult)Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) })
                    : CurrentUmbracoPage();
            }

            return IsAjaxRequest()
                ? (ActionResult)Json(new { success = true })
                : model.NextStep.HasValue
                    ? RedirectToUmbracoPage(model.NextStep.Value)
                    : RedirectToCurrentUmbracoPage();
        }

        private string GetIPAddress()
        {
            var context = System.Web.HttpContext.Current;
            if (context == null) return string.Empty;

            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                    return addresses[0];
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        private bool IsAjaxRequest()
        {
            if (Request.IsAjaxRequest())
                return true;

            var headerName = "X-Requested-With";
            var headerValue = "xmlhttprequest";

            return (Request[headerName] != null && Request[headerName].InvariantEquals(headerValue))
                || (Request.Headers != null && Request.Headers[headerName] != null && Request.Headers[headerName].InvariantEquals(headerValue));
        }
    }
}
