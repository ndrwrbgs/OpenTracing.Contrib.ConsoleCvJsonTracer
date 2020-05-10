namespace OpenTracing.Contrib.ConsoleCvJsonTracer
{
    using System;

    internal static class Demo
    {
        private static void Main()
        {
            var t = CvTextWriterTracerFactory.CreateTracer(Console.Out);

            using (t.BuildSpan("root").StartActive())
            while (true)
            {
                using (var scope = t.BuildSpan("test")
                    .WithTag("a", true)
                    .WithTag("b", 2)
                    .StartActive())
                {
                    using (t.BuildSpan("c1")
                        .StartActive())
                    {
                        using (t.BuildSpan("d1")
                            .StartActive())
                        {
                            t.ActiveSpan.Log("event");
                            t.ActiveSpan.SetTag("tag", true);
                        }
                    }
                }
            }
        }
    }
}