namespace Vendr.Checkout.Pipeline
{
    /// <summary>
    /// Base type for individual pipeline task.
    /// Descendants of this type map an input value to an output value.
    /// The input and output types can differ.
    /// </summary>
    internal interface IPipelineTask<INPUT, OUTPUT>
    {
        OUTPUT Process(INPUT input);
    }
}