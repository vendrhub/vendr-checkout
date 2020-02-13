namespace Vendr.Checkout.Pipeline
{
    /// <summary>
    /// An extension method for combining PipelineTasks together into a data flow.
    /// </summary>
    internal static class PipelineTaskExtensions
    {
        public static OUTPUT Pipe<INPUT, OUTPUT>(this INPUT input, IPipelineTask<INPUT, OUTPUT> task)
        {
            return task.Process(input);
        }
    }
}