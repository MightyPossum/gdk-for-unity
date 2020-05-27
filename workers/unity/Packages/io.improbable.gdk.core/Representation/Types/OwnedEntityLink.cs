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
        public string EntityType { get; }
        public int[] RequiredComponents { get; }

        public GameObject OwnedPrefab;
        public GameObject UnownedPrefab;

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
