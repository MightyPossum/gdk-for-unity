using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Improbable.Gdk.EditmodePerformanceTests
{
    public class ThisShouldRun
    {
        [Performance, Test]
        public void Mono_Default()
        {
            Assert.IsTrue(true);
        }

        [Performance, Test]
        public void Mono_Disabled()
        {
            Assert.IsTrue(true);
        }

        [Performance, Test]
        public void Mono_Any()
        {
            Assert.IsTrue(true);
        }

        [Performance, Test]
        public void Il2Cpp_Default()
        {
            Assert.IsTrue(true);
        }

        [Performance, Test]
        public void Il2Cpp_Disabled()
        {
            Assert.IsTrue(true);
        }

        [Performance, Test]
        public void Il2Cpp_Any()
        {
            Assert.IsTrue(true);
        }

        [Performance, Test]
        public void Any_Default()
        {
            Assert.IsTrue(true);
        }

        [Performance, Test]
        public void Any_Disabled()
        {
            Assert.IsTrue(true);
        }

        [Performance, Test]
        public void Any_Any()
        {
            Assert.IsTrue(true);
        }
    }
}
