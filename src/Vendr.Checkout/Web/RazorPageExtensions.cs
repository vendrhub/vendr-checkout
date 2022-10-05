
using Microsoft.AspNetCore.Mvc.Razor;

namespace Vendr.Checkout.Web
{
    public static class RazorPageExtensions
    {
        public static TService GetService<TService>(this RazorPage view)
        {
            return (TService)view.Context.RequestServices.GetService(typeof(TService));
        }
    }
}
