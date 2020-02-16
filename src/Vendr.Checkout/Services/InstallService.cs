using Vendr.Checkout.Pipeline.Implement;

namespace Vendr.Checkout.Services
{
    public class InstallService
    {
        public void Install(int siteRootNodeId)
        {
            var installContext = new InstallPipelineContext();
            //{
            //    SiteRootNodeId = siteRootNodeId
            //};

            var installPipeline = new InstallPipeline();

            installPipeline.Process(installContext);
        }
    }
}