using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation.Types
{
    [Serializable]
    [NeedsComponent(54)]
    public class OwnedEntityLink : IEntityRepresentation
    {
        public string EntityType => entityType;

        public uint[] RequiredComponents => requiredComponents;

#pragma warning disable 649
        [SerializeField] private string entityType;
        public GameObject OwnedPrefab;
        public GameObject UnownedPrefab;
        [SerializeField] private uint[] requiredComponents;
#pragma warning restore 649

        public GameObject Resolve(SpatialOSEntityInfo entityInfo, EntityManager manager)
        {
            var authComponent = ComponentDatabase.Metaclasses[54].Data; //TODO Change to AUTH
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
