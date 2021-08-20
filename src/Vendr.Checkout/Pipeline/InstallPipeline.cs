using System.Collections.Generic;
using Vendr.Common.Pipelines;

namespace Vendr.Checkout.Pipeline
{
    public class InstallPipeline : PipelineTaskCollection<InstallPipelineContext>
    {
        public InstallPipeline(IEnumerable<IPipelineTask<InstallPipelineContext>> items)
            : base(items)
        { }
    }
}