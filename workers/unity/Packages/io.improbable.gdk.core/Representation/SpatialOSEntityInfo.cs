using Unity.Entities;

namespace Improbable.Gdk.Core.Representation
{
    public readonly struct SpatialOSEntityInfo
    {
        public readonly Entity Entity;
        public readonly EntityId SpatialOSEntityId;

        public SpatialOSEntityInfo(Entity entity, EntityId spatialOSEntityId)
        {
            Entity = entity;
            SpatialOSEntityId = spatialOSEntityId;
        }
    }
}
