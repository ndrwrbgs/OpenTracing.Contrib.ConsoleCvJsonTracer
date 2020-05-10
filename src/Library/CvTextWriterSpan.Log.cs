using System.Collections.Generic;
using Newtonsoft.Json;
namespace OpenTracing.Contrib.ConsoleCvJsonTracer
{
    internal sealed partial class CvTextWriterSpan
    {
        
        public override CvTextWriterSpan Log(IEnumerable<KeyValuePair<string, object>> fields)
        {
            this.OutputData(OutputDataCategory.Log, JsonConvert.SerializeObject(fields));
            return this;
        }

        public override CvTextWriterSpan Log(string @event)
        {
            this.OutputData(OutputDataCategory.Log, @event);
            return this;
        }
    }
}
