namespace OpenTracing.Contrib.ConsoleCvJsonTracer
{
    using System;
    using System.IO;
    using System.Linq;

    using OpenTracing.Contrib.StronglyTyped;
    using OpenTracing.Util;

    internal sealed partial class CvTextWriterTracer : StronglyTypedTracer<CvTextWriterSpanBuilder, MutableSpanContext, AsyncLocalScopeManager, CvTextWriterSpan>
    {
        private static readonly int MaxLengthOfOutputDataCategory = Enum.GetValues(typeof(OutputDataCategory))
            .OfType<OutputDataCategory>()
            .Select(o => o.ToString())
            .Select(s => s.Length)
            .Max();

        private readonly TextWriter writer;
        private int longestCvStringLengthSeenSoFar = 0;
        private readonly object longestCvStringLengthSeenSoFarLock = new object();

        public CvTextWriterTracer(TextWriter writer, FeatureFlags featureFlags)
        {
            this.writer = writer;
            this.FeatureFlags = featureFlags;
        }

        internal FeatureFlags FeatureFlags { get; }

        public override AsyncLocalScopeManager ScopeManager { get; } = new AsyncLocalScopeManager();

        public override CvTextWriterSpan ActiveSpan => this.ScopeManager.Active.Span as CvTextWriterSpan;

        public override CvTextWriterSpanBuilder BuildSpan(string operationName)
        {
            return new CvTextWriterSpanBuilder(this, this.ScopeManager, operationName);
        }
        
        internal void OutputData(
            string fullCorrelationVector,
            OutputDataCategory outputDataCategory,
            string data)
        {
            var cVToOutput = fullCorrelationVector;

            if (this.FeatureFlags.ChopLastDigitOfCv)
            {
                // We exclude the last part, since that's used for the increment and to know what the NEXT child will be - we find it more natural to only show the user N-1 parts
                var cvLastPeriod = cVToOutput.LastIndexOf('.');
                var cvMinusLast = cVToOutput.Substring(0, cvLastPeriod);

                cVToOutput = cvMinusLast;
            }

            if (!this.FeatureFlags.OutputTraceId)
            {
                var cvFirstPeriod = cVToOutput.IndexOf('.');
                if (cvFirstPeriod == -1)
                {
                    cVToOutput = string.Empty;
                }
                else
                {
                    cVToOutput = cVToOutput.Substring(cvFirstPeriod);
                }
            }

            if (this.FeatureFlags.PrefixCvForOutput != null)
            {
                cVToOutput = this.FeatureFlags.PrefixCvForOutput + cVToOutput;
            }


            UpdateLongestCvStringLengthSeenSoFar(cVToOutput);

            this.writer.WriteLine($"{cVToOutput.PadRight(this.longestCvStringLengthSeenSoFar)} | {outputDataCategory.ToString().PadRight(MaxLengthOfOutputDataCategory)} | {data}");
        }

        private void UpdateLongestCvStringLengthSeenSoFar(string seen)
        {
            var len = seen.Length;
            if (len > this.longestCvStringLengthSeenSoFar)
            {
                lock (this.longestCvStringLengthSeenSoFarLock)
                {
                    // Double-check lock
                    if (len > this.longestCvStringLengthSeenSoFar)
                    {
                        this.longestCvStringLengthSeenSoFar = len;
                    }
                }
            }
        }
    }
}