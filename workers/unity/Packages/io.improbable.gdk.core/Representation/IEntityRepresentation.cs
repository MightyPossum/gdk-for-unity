using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation
{
    public interface IEntityRepresentation : IDeclareReferencedPrefabs
    {
        // The type of the entity (meta-data string)
        string EntityType { get; }

        // Required components before spawning (same as the current `EntityTypeExpectations`)
        IEnumerable<uint> RequiredComponents { get; }

        GameObject Resolve(SpatialOSEntityInfo entity, EntityManager manager);
    }
}
