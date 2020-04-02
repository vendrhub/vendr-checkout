using Vendr.Checkout.Pipeline.Implement;
using Vendr.Core.Models;

namespace Vendr.Checkout.Services
{
    public class InstallService
    {
        public void Install(int siteRootNodeId, StoreReadOnly store)
        {
            var installPipeline = new InstallPipeline();

            installPipeline.Process(new InstallPipelineContext
            {
                SiteRootNodeId = siteRootNodeId,
                Store = store
            });
        }
    }
}