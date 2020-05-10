namespace OpenTracing.Contrib.ConsoleCvJsonTracer
{
    using System.IO;

    public static class CvTextWriterTracerFactory
    {
        public static ITracer CreateTracer(TextWriter textWriter)
        {
            return new CvTextWriterTracer(textWriter, new FeatureFlags(
                                              cvsStartAt1: true,
                                              chopLastDigitOfCv: true,
                                              outputTraceId: true,
                                              prefixCvForOutput: null));
        }
        public static ITracer CreateTracer(TextWriter textWriter, FeatureFlags featureFlags)
        {
            return new CvTextWriterTracer(textWriter, featureFlags);
        }
    }
}