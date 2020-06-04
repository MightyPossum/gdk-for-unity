using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.PlayerLifecycle.Representation.Types
{
    [Serializable]
    public class OwningWorkerEntityLink : IEntityRepresentation
    {
        public string EntityType => entityType;
        public IEnumerable<uint> RequiredComponents => requiredComponents.Append(OwningWorker.ComponentId);

#pragma warning disable 649
        [SerializeField] private string entityType;
        [SerializeField] private GameObject OwnedPrefab;
        [SerializeField] private GameObject UnownedPrefab;
        [SerializeField] private uint[] requiredComponents;
#pragma warning restore 649

        public GameObject Resolve(SpatialOSEntityInfo entityInfo, EntityManager manager)
        {
            var owningWorkerId = manager.GetComponentData<OwningWorker.Component>(entityInfo.Entity).WorkerId;
            var myWorkerId = manager.World.GetExistingSystem<WorkerSystem>().WorkerId;

            return owningWorkerId == myWorkerId
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
