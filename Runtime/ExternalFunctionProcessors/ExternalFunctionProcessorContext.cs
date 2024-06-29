using System;
using System.Runtime.Serialization;

namespace LemuRivolta.InkAtoms.ExternalFunctionProcessors
{


    /// <summary>
    ///     Base class for <see cref="ExternalFunctionProcessorContextWithResult" /> that doesn't wrap the result.
    /// </summary>
    public class ExternalFunctionProcessorContext : ParametersBag
    {
        protected ExternalFunctionProcessorContext(object[] parameters)
        {
            AddParameters(parameters);
        }
    }

    
}