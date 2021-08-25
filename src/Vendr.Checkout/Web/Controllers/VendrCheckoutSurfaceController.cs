using System.Linq;
using System.Collections.Generic;
using Vendr.Checkout.Web.Dtos;
using Vendr.Core.Api;
using Vendr.Extensions;
using Vendr.Common.Validation;
using VendrConstants = Vendr.Core.Constants;

#if NETFRAMEWORK
using Umbraco.Core;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using IActionResult = System.Web.Mvc.ActionResult;
#else
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Extensions;
#endif

namespace Vendr.Checkout.Web.Controllers
{
    public class VendrCheckoutSurfaceController : SurfaceController
    {
        private readonly IVendrApi _vendrApi;

#if NETFRAMEWORK
        public VendrCheckoutSurfaceController(IVendrApi vendrAPi)
#else
        public VendrCheckoutSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, 
            ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider,
            IVendrApi vendrAPi)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
#endif
        {
            _vendrApi = vendrAPi;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#if NET
        [ValidateUmbracoFormRouteString]
#endif
        public IActionResult ApplyDiscountOrGiftCardCode(VendrDiscountOrGiftCardCodeDto model)
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
                ModelState.AddModelError("code", "Failed to redeem discount code: "+ ex.Message);

                return IsAjaxRequest()
                    ? (IActionResult)Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) })
                    : CurrentUmbracoPage();
            }

            return IsAjaxRequest()
                ? (IActionResult)Json(new { success = true })
                : RedirectToCurrentUmbracoPage();
        }

        [HttpGet]
#if NET
        [ValidateUmbracoFormRouteString]
#endif
        public IActionResult RemoveDiscountOrGiftCardCode(VendrDiscountOrGiftCardCodeDto model)
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
                    ? (IActionResult)JsonGet(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) })
                    : CurrentUmbracoPage();
            }

            return IsAjaxRequest()
                ? (IActionResult)JsonGet(new { success = true })
                : RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#if NET
        [ValidateUmbracoFormRouteString]
#endif
        public IActionResult UpdateOrderInformation(VendrUpdateOrderInformationDto model)
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
                    ? (IActionResult)Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) })
                    : CurrentUmbracoPage();
            }

            return IsAjaxRequest()
                ? (IActionResult)Json(new { success = true })
                : model.NextStep.HasValue
                    ? RedirectToUmbracoPage(model.NextStep.Value)
                    : RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#if NET
        [ValidateUmbracoFormRouteString]
#endif
        public IActionResult UpdateOrderShippingMethod(VendrUpdateOrderShippingMethodDto model)
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
                    ? (IActionResult)Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) })
                    : CurrentUmbracoPage();
            }

            return IsAjaxRequest()
                ? (IActionResult)Json(new { success = true })
                : model.NextStep.HasValue
                    ? RedirectToUmbracoPage(model.NextStep.Value)
                    : RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
#if NET
        [ValidateUmbracoFormRouteString]
#endif
        public IActionResult UpdateOrderPaymentMethod(VendrUpdateOrderPaymentMethodDto model)
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
                    ? (IActionResult)Json(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage) })
                    : CurrentUmbracoPage();
            }

            return IsAjaxRequest()
                ? (IActionResult)Json(new { success = true })
                : model.NextStep.HasValue
                    ? RedirectToUmbracoPage(model.NextStep.Value)
                    : RedirectToCurrentUmbracoPage();
        }

        private string GetIPAddress()
        {
            var ipAddress = HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR");
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                    return addresses[0];
            }

            return HttpContext.GetServerVariable("REMOTE_ADDR");
        }

        private bool IsAjaxRequest()
        {
            var headerName = "X-Requested-With";
            var headerValue = "xmlhttprequest";

#if NETFRAMEWORK
            return (Request[headerName] != null && Request[headerName].InvariantEquals(headerValue))
                || (Request.Headers != null && Request.Headers[headerName] != null && Request.Headers[headerName].InvariantEquals(headerValue));
#else
            return (Request.Query.ContainsKey(headerName) && Request.Query[headerName].ToString().InvariantEquals(headerValue))
                || (Request.Form.ContainsKey(headerName) && Request.Form[headerName].ToString().InvariantEquals(headerValue))
                || (Request.Headers != null && Request.Headers.ContainsKey(headerName) && Request.Headers[headerName].ToString().InvariantEquals(headerValue));
#endif
        }

        private JsonResult JsonGet(object model)
#if NETFRAMEWORK
            => Json(model, JsonRequestBehavior.AllowGet);
#else
            => Json(model);
#endif
    }
}
