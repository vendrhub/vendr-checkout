using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vendr.Checkout.Pipeline.Implement.Tasks
{
    internal class ConfigureVendrStoreTask : IPipelineTask<InstallPipelineContext, InstallPipelineContext>
    {
        public InstallPipelineContext Process(InstallPipelineContext ctx)
        {
            

            // Continue the pipeline
            return ctx;
        }
    }
}