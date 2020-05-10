namespace OpenTracing.Contrib.ConsoleCvJsonTracer {
    using System.Collections.Generic;
    using System.Collections.Immutable;

    using Microsoft.CorrelationVector;

    internal sealed class MutableSpanContext : ISpanContext
    {
        private ImmutableDictionary<string, string> baggageItems;

        public MutableSpanContext(MutableSpanContext parentContext)
        {
            this.baggageItems = parentContext?.baggageItems ?? ImmutableDictionary<string, string>.Empty;
        }

        public IEnumerable<KeyValuePair<string, string>> GetBaggageItems()
        {
            return this.baggageItems;
        }

        public string TraceId
        {
            get
            {
                if (!this.baggageItems.TryGetValue(Constants.CorrelationVectorKeyInBaggageItems, out string cv))
                {
                    return null;
                }

                var parsed = CorrelationVector.Parse(cv);
                return parsed.GetBaseAsGuid().ToString();
            }
        }

        public string SpanId =>
            this.baggageItems.TryGetValue(Constants.CorrelationVectorKeyInBaggageItems, out string cv) 
                ? cv 
                : null;

        internal void SetBaggageItem(string key, string value)
        {
            this.baggageItems = this.baggageItems.SetItem(key, value);
        }

        internal string GetBaggageItem(string key)
        {
            return this.baggageItems[key];
        }
    }
}