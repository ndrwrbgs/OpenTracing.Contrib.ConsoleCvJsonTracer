
namespace OpenTracing.Contrib.ConsoleCvJsonTracer
{
    using System;

    using OpenTracing.Contrib.StronglyTyped;

    internal sealed partial class CvTextWriterSpan : StronglyTypedSpan<CvTextWriterSpan, MutableSpanContext>
    {
        private readonly CvTextWriterTracer tracer;

        public CvTextWriterSpan(CvTextWriterTracer tracer, MutableSpanContext parentContext)
        {
            this.tracer = tracer;
            this.Context = new MutableSpanContext(parentContext);
        }

        public override MutableSpanContext Context { get; }

        private void OutputData(OutputDataCategory outputType, string data)
        {
            var cv = this.GetBaggageItem(Constants.CorrelationVectorKeyInBaggageItems);
            this.tracer.OutputData(cv, outputType, data);
        }

        public override CvTextWriterSpan SetBaggageItem(string key, string value)
        {
            this.Context.SetBaggageItem(key, value);
            return this;
        }

        public override string GetBaggageItem(string key)
        {
            return this.Context.GetBaggageItem(key);
        }

        public override CvTextWriterSpan SetOperationName(string operationName)
        {
            throw new NotSupportedException("You cannot retroactively set the operation name on this tracer");
        }

        public override void Finish()
        {
            this.OutputData(OutputDataCategory.Finish, string.Empty);
        }
    }
}