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
        public int[] RequiredComponents { get; }

        [SerializeField] private string entityType;
        public GameObject Prefab;

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
