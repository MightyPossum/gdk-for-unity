using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation
{
    [CreateAssetMenu(menuName = "SpatialOS/Entity Linker")]
    public class EntityLinkerDatabase : ScriptableObject
    {
        [SerializeReference] public List<IEntityRepresentation> EntityRepresentationResolvers = new List<IEntityRepresentation>();
    }
}
