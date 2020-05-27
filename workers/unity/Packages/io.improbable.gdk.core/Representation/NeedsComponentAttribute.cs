using System;

namespace Improbable.Gdk.Core.Representation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NeedsComponentAttribute : Attribute
    {
        public readonly uint ComponentId;

        public NeedsComponentAttribute(uint componentId)
        {
            ComponentId = componentId;
        }
    }
}
