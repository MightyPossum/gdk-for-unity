using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Improbable.Gdk.EditmodePerformanceTests
{
    public class ThisShouldNotRun
    {
        [Performance, Test]
        public void Mono_Burst()
        {
            Assert.IsTrue(false);
        }

        [Performance, Test]
        [Il2Cpp]
        public void Il2cpp_Burst()
        {
            Assert.IsTrue(false);
        }

        [Performance, Test]
        [BurstOff]
        public void Mono_Burstoff()
        {
            Assert.IsTrue(false);
        }

        [Performance, Test]
        [Il2Cpp, BurstOff]
        public void Il2cpp_Burstoff()
        {
            Assert.IsTrue(false);
        }
    }
}
