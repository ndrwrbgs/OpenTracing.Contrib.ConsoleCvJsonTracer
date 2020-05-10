using System;
using System.Collections.Generic;
namespace OpenTracing.Contrib.ConsoleCvJsonTracer
{
    internal sealed partial class CvTextWriterSpan
    {
        public override void Finish(DateTimeOffset finishTimestamp)
        {
            throw new NotSupportedException("This tracer expected Finish to match wall-time, since it's live streaming the trace to output");
        }
        

        public override CvTextWriterSpan Log(DateTimeOffset timestamp, IEnumerable<KeyValuePair<string, object>> fields)
        {
            throw new NotSupportedException("This tracer expected Log to match wall-time, since it's live streaming the trace to output");
        }

        public override CvTextWriterSpan Log(DateTimeOffset timestamp, string @event)
        {
            throw new NotSupportedException("This tracer expected Log to match wall-time, since it's live streaming the trace to output");
        }
    }
}
