#if NETFRAMEWORK
using Umbraco.Core.Composing;
using RazorPage = System.Web.Mvc.WebViewPage;
#else
using Microsoft.AspNetCore.Mvc.Razor;
#endif

namespace Vendr.Checkout.Web
{
    public static class RazorPageExtensions
    {
        public static TService GetService<TService>(this RazorPage view)
        {
#if NETFRAMEWORK
            return (TService)Current.Factory.GetInstance(typeof(TService));
#else
            return (TService)view.Context.RequestServices.GetService(typeof(TService));
#endif
        }
    }
}
