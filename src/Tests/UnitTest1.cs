using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    using OpenTracing;
    using OpenTracing.Util;

    [TestClass]
    public class DependencyAssertions
    {
        [TestMethod]
        public void AsyncLocalScopeManager_Active_ReturnsInput()
        {
            var span = NSubstitute.Substitute.For<ISpan>();

            AsyncLocalScopeManager scopeManager = new AsyncLocalScopeManager();
            scopeManager.Activate(span, true);

            var active = scopeManager.Active.Span;

            Assert.AreSame(span, active);
        }

        [TestMethod]
        public void AsyncLocalScopeManager_Activate_ReturnsAsyncLocalScope()
        {
            var span = NSubstitute.Substitute.For<ISpan>();

            AsyncLocalScopeManager scopeManager = new AsyncLocalScopeManager();
            var scope = scopeManager.Activate(span, true);

            Assert.IsInstanceOfType(scope, typeof(AsyncLocalScope));
        }
    }
}
