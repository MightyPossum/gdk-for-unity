using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation
{
    [CreateAssetMenu(menuName = "SpatialOS/Entity Linker")]
    public class EntityLinkerDatabase : ScriptableObject
    {
        [SerializeReference] internal List<IEntityRepresentation> entityRepresentationResolvers = new List<IEntityRepresentation>();

        private Dictionary<string, IEntityRepresentation> entityLookup;

        private void RefreshEntityLookup()
        {
            entityLookup = entityRepresentationResolvers.ToDictionary(
                representation => representation.EntityType,
                representation => representation);
        }

        public IEntityRepresentation GetEntityResolver(string entityType)
        {
            RefreshEntityLookup();

            if (!entityLookup.TryGetValue(entityType, out var representation))
            {
                return null;
            }

            return representation;
        }
    }
}
