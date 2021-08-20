using Vendr.Checkout.Pipeline;
using Vendr.Core.Models;
using PipelineRunner = Vendr.Common.Pipelines.Pipeline;

namespace Vendr.Checkout.Services
{
    public class InstallService
    {
        public void Install(int siteRootNodeId, StoreReadOnly store)
        {
            var result = PipelineRunner.Invoke<InstallPipeline, InstallPipelineContext>(new InstallPipelineContext
            {
                SiteRootNodeId = siteRootNodeId,
                Store = store
            });

            if (!result.Success)
                throw result.Exception;
        }
    }
}