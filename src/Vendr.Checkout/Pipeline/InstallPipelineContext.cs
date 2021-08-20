using Vendr.Core.Models;

namespace Vendr.Checkout.Pipeline
{
    public class InstallPipelineContext
    {
        public int SiteRootNodeId { get; set; }

        public StoreReadOnly Store { get; set; }

        public string CartPageUrl { get; set; }

        public string ConfirmationPageUrl { get; set; }
    }
}