namespace OpenTracing.Contrib.ConsoleCvJsonTracer
{
    using System;
    using OpenTracing.Propagation;

    internal sealed partial class CvTextWriterTracer
    {
        public override MutableSpanContext Extract<TCarrier>(IFormat<TCarrier> format, TCarrier carrier)
        {
            throw new NotSupportedException("Wire format is not supported");
        }

        public override void Inject<TCarrier>(MutableSpanContext spanContext, IFormat<TCarrier> format, TCarrier carrier)
        {
            throw new NotSupportedException("Wire format is not supported");
        }
    }
}