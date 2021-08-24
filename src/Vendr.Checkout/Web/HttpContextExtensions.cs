using System.Web;

namespace Vendr.Checkout.Web
{
    internal static class HttpContextExtensions
    {
#if NETFRAMEWORK
        public static string GetServerVariable(this HttpContextBase ctx, string variableName)
            => ctx.Request.ServerVariables[variableName];
#endif
    }
}
