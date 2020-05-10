using OpenTracing.Tag;
namespace OpenTracing.Contrib.ConsoleCvJsonTracer
{
    internal sealed partial class CvTextWriterSpan
    {
        public override CvTextWriterSpan SetTag(string key, string value)
        {
            // TODO: These are not actually properly json encoded
            this.OutputData(OutputDataCategory.Tag, $"{key}: \"{value}\"");
            return this;
        }

        public override CvTextWriterSpan SetTag(string key, bool value)
        {
            this.OutputData(OutputDataCategory.Tag, $"{key}: {value}");
            return this;
        }

        public override CvTextWriterSpan SetTag(string key, int value)
        {
            this.OutputData(OutputDataCategory.Tag, $"{key}: {value}");
            return this;
        }

        public override CvTextWriterSpan SetTag(string key, double value)
        {
            this.OutputData(OutputDataCategory.Tag, $"{key}: {value}");
            return this;
        }

        public override CvTextWriterSpan SetTag(BooleanTag tag, bool value)
        {
            this.OutputData(OutputDataCategory.Tag, $"{tag.Key}: {value}");
            return this;
        }

        public override CvTextWriterSpan SetTag(IntOrStringTag tag, string value)
        {
            this.OutputData(OutputDataCategory.Tag, $"{tag.Key}: \"{value}\"");
            return this;
        }

        public override CvTextWriterSpan SetTag(IntTag tag, int value)
        {
            this.OutputData(OutputDataCategory.Tag, $"{tag.Key}: {value}");
            return this;
        }

        public override CvTextWriterSpan SetTag(StringTag tag, string value)
        {
            this.OutputData(OutputDataCategory.Tag, $"{tag.Key}: \"{value}\"");
            return this;
        }
    }
}
