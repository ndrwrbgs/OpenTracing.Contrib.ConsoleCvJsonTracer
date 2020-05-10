
namespace OpenTracing.Contrib.ConsoleCvJsonTracer {
    public sealed class FeatureFlags
    {
        public bool CvsStartAt1 { get; }

        public bool ChopLastDigitOfCv { get; }

        public bool OutputTraceId { get; }

        public string PrefixCvForOutput { get; }

        public FeatureFlags(string prefixCvForOutput = null, bool cvsStartAt1 = true, bool chopLastDigitOfCv = true, bool outputTraceId = false)
        {
            this.PrefixCvForOutput = prefixCvForOutput;
            this.CvsStartAt1 = cvsStartAt1;
            this.ChopLastDigitOfCv = chopLastDigitOfCv;
            this.OutputTraceId = outputTraceId;
        }
    }
}