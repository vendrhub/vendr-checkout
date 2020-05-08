using System;

namespace Vendr.Checkout.Pipeline
{
    /// <summary>
    /// The base type for a complete pipeline.
    /// Descendant types can use their constructor to compile a set of PipelineSteps together
    /// the PipelineStepExtensions.Step() method, and assign this to the PipelineSteps property here.
    /// The initial and final types of the set of steps must match the input and output types of this class,
    /// but the intermediate types can vary.
    /// </summary>
    internal abstract class Pipeline<INPUT, OUTPUT> : IPipelineTask<INPUT, OUTPUT>
    {
        public Func<INPUT, OUTPUT> Tasks { get; protected set; }

        public OUTPUT Process(INPUT input)
        {
            return Tasks(input);
        }
    }
}