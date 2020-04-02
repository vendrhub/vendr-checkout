using Vendr.Checkout.Pipeline.Implement.Tasks;

namespace Vendr.Checkout.Pipeline.Implement
{
    internal class InstallPipeline : Pipeline<InstallPipelineContext, InstallPipelineContext>
    {
        public InstallPipeline()
        {
            Tasks = input => input
                .Pipe(new CreateVendrCheckoutDataTypesTask())
                .Pipe(new CreateVendrCheckoutDocumentTypesTask());
                //.Pipe(new CreateVendrCheckoutNodesTask());
                //.Pipe(new ConfigureVendrStoreTask());
        }
    }
}