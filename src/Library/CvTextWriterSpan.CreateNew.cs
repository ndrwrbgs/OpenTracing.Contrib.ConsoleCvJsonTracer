using System.Collections.Immutable;
using System.Linq;

using Microsoft.CorrelationVector;

using Newtonsoft.Json;
using OpenTracing.Util;
namespace OpenTracing.Contrib.ConsoleCvJsonTracer
{
    internal sealed partial class CvTextWriterSpan
    {
        public static CvTextWriterSpan CreateNew(
            CvTextWriterTracer tracer,
            AsyncLocalScopeManager scopeManager,
            string operationName,
            ImmutableList<(string referenceType, MutableSpanContext context)> references,
            bool ignoreActiveSpan,
            ImmutableDictionary<string, object> initialTags,
            MutableSpanContext parentContext)
        {
            if (parentContext == null && !ignoreActiveSpan)
            {
                parentContext = (MutableSpanContext) scopeManager.Active?.Span.Context;
            }
            
            CorrelationVector childCv;
            if (parentContext != null)
            {
                // Extend and increment
                CorrelationVector parentCv = CorrelationVector.Parse(parentContext.GetBaggageItem(Constants.CorrelationVectorKeyInBaggageItems));

                childCv = CorrelationVector.Extend(parentCv.Value);

                var newParentCv = CorrelationVector.Parse(parentCv.Increment());
                parentContext.SetBaggageItem(Constants.CorrelationVectorKeyInBaggageItems, newParentCv.Value);
            }
            else
            {
                // New
                childCv = new CorrelationVector(CorrelationVectorVersion.V2);
            }

            if (tracer.FeatureFlags.CvsStartAt1)
            {
                childCv = CorrelationVector.Parse(childCv.Increment());
            }

            var span = new CvTextWriterSpan(tracer, parentContext);
            span.SetBaggageItem(Constants.CorrelationVectorKeyInBaggageItems, childCv.Value);

            if (initialTags != null && initialTags.Any())
            {
                span.OutputData(OutputDataCategory.Start, $"Operation '{operationName}' with tags {JsonConvert.SerializeObject(initialTags)}");
            }
            else
            {
                span.OutputData(OutputDataCategory.Start, $"Operation '{operationName}'");
            }

            if (references != null && references.Any())
            {
                foreach ((string referenceType, MutableSpanContext context) in references)
                {
                    span.OutputData(OutputDataCategory.Reference, JsonConvert.SerializeObject(new { referenceType, cV = context.GetBaggageItem(Constants.CorrelationVectorKeyInBaggageItems) }));
                }
            }

            return span;
        }
    }
}
