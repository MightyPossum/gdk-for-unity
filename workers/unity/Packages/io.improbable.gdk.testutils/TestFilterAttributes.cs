using System;
using NUnit.Framework;

namespace Improbable.Gdk.TestUtils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class BaseFilterAttribute : CategoryAttribute
    {
        protected BaseFilterAttribute(string category) : base(category) { }
    }

    public class Il2CppAttribute : BaseFilterAttribute
    {
        public Il2CppAttribute() : base("Il2CppBackend") { }
    }

    public class BurstOffAttribute : BaseFilterAttribute
    {
        public BurstOffAttribute() : base("BurstOff") { }
    }
}
