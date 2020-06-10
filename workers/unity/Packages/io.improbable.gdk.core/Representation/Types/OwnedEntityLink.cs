using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation.Types
{
    [Serializable]
    public class OwnedEntityLink : IEntityRepresentation
    {
        public string EntityType => entityType;
        public IEnumerable<uint> RequiredComponents => requiredComponents.Append(authComponentId);

#pragma warning disable 649
        [SerializeField] private string entityType;
        [SerializeField] private GameObject OwnedPrefab;
        [SerializeField] private GameObject UnownedPrefab;
        [SerializeField] private uint authComponentId;
        [SerializeField] private uint[] requiredComponents;
#pragma warning restore 649

        public GameObject Resolve(SpatialOSEntityInfo entityInfo, EntityManager manager)
        {
            var authComponent = ComponentDatabase.GetMetaclass(authComponentId).Authority;
            return manager.HasComponent(entityInfo.Entity, authComponent)
                ? OwnedPrefab
                : UnownedPrefab;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(OwnedPrefab);
            referencedPrefabs.Add(UnownedPrefab);
        }
    }
}
