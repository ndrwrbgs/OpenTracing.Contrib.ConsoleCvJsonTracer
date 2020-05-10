namespace OpenTracing.Contrib.ConsoleCvJsonTracer
{
    using System;
    using System.Collections.Immutable;

    using OpenTracing.Contrib.StronglyTyped;
    using OpenTracing.Tag;
    using OpenTracing.Util;

    internal class CvTextWriterSpanBuilder : StronglyTypedSpanBuilder<CvTextWriterSpanBuilder, MutableSpanContext, CvTextWriterSpan, AsyncLocalScope>
    {
        private readonly CvTextWriterTracer tracer;
        private readonly AsyncLocalScopeManager scopeManager;
        private readonly string operationName;
        private readonly bool ignoreActiveSpan;
        private readonly MutableSpanContext parentContext;
        private readonly ImmutableList<(string referenceType, MutableSpanContext context)> references;
        private readonly ImmutableDictionary<string, object> initialTags;

        public CvTextWriterSpanBuilder(CvTextWriterTracer tracer, AsyncLocalScopeManager scopeManager, string operationName, bool ignoreActiveSpan = false, ImmutableList<(string referenceType, MutableSpanContext context)> references = null, ImmutableDictionary<string, object> initialTags = null, MutableSpanContext parentContext = null)
        {
            this.tracer = tracer;
            this.scopeManager = scopeManager;
            this.operationName = operationName;
            this.ignoreActiveSpan = ignoreActiveSpan;
            this.parentContext = parentContext;
            this.references = references ?? ImmutableList<(string referenceType, MutableSpanContext context)>.Empty;
            this.initialTags = initialTags ?? ImmutableDictionary<string, object>.Empty;
        }

        public override CvTextWriterSpanBuilder AsChildOf(MutableSpanContext newParent)
        {
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.ignoreActiveSpan,
                this.references,
                this.initialTags,
                newParent);
        }

        public override CvTextWriterSpanBuilder AsChildOf(CvTextWriterSpan newParent)
        {
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.ignoreActiveSpan,
                this.references,
                this.initialTags,
                newParent.Context);
        }

        public override CvTextWriterSpanBuilder AddReference(string referenceType, MutableSpanContext referencedContext)
        {
            if (referenceType == References.ChildOf)
            {
                return new CvTextWriterSpanBuilder(
                    this.tracer,
                    this.scopeManager,
                    this.operationName,
                    this.ignoreActiveSpan,
                    this.references,
                    this.initialTags,
                    referencedContext);
            }
            
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.ignoreActiveSpan,
                this.references.Add((referenceType, referencedContext)),
                this.initialTags,
                this.parentContext);
        }

        public override CvTextWriterSpanBuilder IgnoreActiveSpan()
        {
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                true,
                this.references,
                this.initialTags,
                this.parentContext);
        }

        public override CvTextWriterSpanBuilder WithTag(string key, string value)
        {
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.ignoreActiveSpan,
                this.references,
                this.initialTags.Add(key, value),
                this.parentContext);
        }

        public override CvTextWriterSpanBuilder WithTag(string key, bool value)
        {
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.ignoreActiveSpan,
                this.references,
                this.initialTags.Add(key, value),
                this.parentContext);
        }

        public override CvTextWriterSpanBuilder WithTag(string key, int value)
        {
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.ignoreActiveSpan,
                this.references,
                this.initialTags.Add(key, value),
                this.parentContext);
        }

        public override CvTextWriterSpanBuilder WithTag(string key, double value)
        {
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.ignoreActiveSpan,
                this.references,
                this.initialTags.Add(key, value),
                this.parentContext);
        }

        public override CvTextWriterSpanBuilder WithTag(BooleanTag tag, bool value)
        {
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.ignoreActiveSpan,
                this.references,
                this.initialTags.Add(tag.Key, value),
                this.parentContext);
        }

        public override CvTextWriterSpanBuilder WithTag(IntOrStringTag tag, string value)
        {
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.ignoreActiveSpan,
                this.references,
                this.initialTags.Add(tag.Key, value),
                this.parentContext);
        }

        public override CvTextWriterSpanBuilder WithTag(IntTag tag, int value)
        {
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.ignoreActiveSpan,
                this.references,
                this.initialTags.Add(tag.Key, value),
                this.parentContext);
        }

        public override CvTextWriterSpanBuilder WithTag(StringTag tag, string value)
        {
            return new CvTextWriterSpanBuilder(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.ignoreActiveSpan,
                this.references,
                this.initialTags.Add(tag.Key, value),
                this.parentContext);
        }

        public override CvTextWriterSpanBuilder WithStartTimestamp(DateTimeOffset timestamp)
        {
            throw new NotSupportedException("This tracer expected StartTimestamp to match wall-time, since it's live streaming the trace to output");
        }

        public override AsyncLocalScope StartActive()
        {
            return this.StartActive(true);
        }

        public override AsyncLocalScope StartActive(bool finishSpanOnDispose)
        {
            return (AsyncLocalScope) this.scopeManager.Activate(this.Start(), finishSpanOnDispose);
        }

        public override CvTextWriterSpan Start()
        {
            return CvTextWriterSpan.CreateNew(
                this.tracer,
                this.scopeManager,
                this.operationName,
                this.references,
                this.ignoreActiveSpan,
                this.initialTags,
                this.parentContext);
        }
    }
}