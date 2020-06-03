using System.Collections.Generic;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation
{
    [CreateAssetMenu(menuName = "SpatialOS/Entity Linker")]
    public class EntityLinkerDatabase : ScriptableObject
    {
        [SerializeReference] public List<IEntityRepresentation> EntityRepresentationResolvers
            = new List<IEntityRepresentation>();
    }
}
