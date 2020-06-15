using Improbable.Gdk.TestUtils;
using NUnit.Framework;

namespace Improbable.Gdk.PlaymodePerformanceTests
{
    public class ThisShouldNotRun
    {
        [PerformanceTest(Burst.Default, Backend.Mono), Test]
        public void Mono_Default()
        {
            Assert.IsTrue(true);
        }

        [PerformanceTest(Burst.Disabled, Backend.Mono), Test]
        public void Mono_Disabled()
        {
            Assert.IsTrue(true);
        }

        [PerformanceTest(Burst.Default | Burst.Disabled, Backend.Mono), Test]
        public void Mono_Any()
        {
            Assert.IsTrue(true);
        }

        [PerformanceTest(Burst.Default, Backend.Il2Cpp), Test]
        public void Il2Cpp_Default()
        {
            Assert.IsTrue(true);
        }

        [PerformanceTest(Burst.Disabled, Backend.Il2Cpp), Test]
        public void Il2Cpp_Disabled()
        {
            Assert.IsTrue(true);
        }

        [PerformanceTest(Burst.Default | Burst.Disabled, Backend.Il2Cpp), Test]
        public void Il2Cpp_Any()
        {
            Assert.IsTrue(true);
        }

        [PerformanceTest(Burst.Default, Backend.Mono | Backend.Il2Cpp), Test]
        public void Any_Default()
        {
            Assert.IsTrue(true);
        }

        [PerformanceTest(Burst.Disabled, Backend.Mono | Backend.Il2Cpp), Test]
        public void Any_Disabled()
        {
            Assert.IsTrue(true);
        }

        [PerformanceTest(Burst.Default | Burst.Disabled, Backend.Mono | Backend.Il2Cpp), Test]
        public void Any_Any()
        {
            Assert.IsTrue(true);
        }
    }
}
