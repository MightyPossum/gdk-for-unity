using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.Core.Representation.Types
{
    [Serializable]
    public class SimpleEntityLink : IEntityRepresentation
    {
        public string EntityType => entityType;
        public int[] RequiredComponents => requiredComponents;

#pragma warning disable 649
        [SerializeField] private string entityType;
        public GameObject Prefab;
        [SerializeField] private int[] requiredComponents;
#pragma warning restore 649

        public GameObject Resolve(SpatialOSEntityInfo entity, EntityManager manager)
        {
            return Prefab;
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(Prefab);
        }
    }
}
