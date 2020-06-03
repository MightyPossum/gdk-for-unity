using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Improbable.Gdk.GameObjectCreation
{
    public class GameObjectCreatorFromMetadata : IEntityGameObjectCreator
    {
        private readonly Dictionary<string, IEntityRepresentation> entityLookup
            = new Dictionary<string, IEntityRepresentation>();

        private readonly string workerType;
        private readonly Vector3 workerOrigin;

        private readonly Dictionary<EntityId, GameObject> entityIdToGameObject = new Dictionary<EntityId, GameObject>();

        private readonly Type[] componentsToAdd =
        {
            typeof(Transform), typeof(Rigidbody), typeof(MeshRenderer)
        };

        public GameObjectCreatorFromMetadata(string workerType, Vector3 workerOrigin, EntityLinkerDatabase entityLinkerDatabase)
        {
            this.workerType = workerType;
            this.workerOrigin = workerOrigin;

            entityLookup = entityLinkerDatabase.EntityRepresentationResolvers.ToDictionary(
                representation => representation.EntityType,
                representation => representation);
        }

        public void PopulateEntityTypeExpectations(EntityTypeExpectations entityTypeExpectations)
        {
            entityTypeExpectations.RegisterDefault(new[]
            {
                typeof(Position.Component)
            });

            foreach (var entityRepresentation in entityLookup)
            {
                var entityType = entityRepresentation.Key;
                var componentTypes = entityRepresentation.Value.RequiredComponents
                    .Select(componentId => ComponentDatabase.GetMetaclass(componentId).Data)
                    .Append(typeof(Position.Component))
                    .Distinct();

                entityTypeExpectations.RegisterEntityType(entityType, componentTypes);
            }
        }

        public void OnEntityCreated(string entityType, SpatialOSEntityInfo entityInfo, EntityManager entityManager, EntityGameObjectLinker linker)
        {
            if (!entityLookup.TryGetValue(entityType, out var representation))
            {
                return;
            }

            var prefab = representation.Resolve(entityInfo, entityManager);
            if (prefab == null)
            {
                return;
            }

            var spatialOSPosition = entityManager.GetComponentData<Position.Component>(entityInfo.Entity);
            var position = spatialOSPosition.Coords.ToUnityVector() + workerOrigin;

            var gameObject = Object.Instantiate(prefab, position, Quaternion.identity);
            gameObject.name = $"{prefab.name}(SpatialOS: {entityInfo.SpatialOSEntityId}, Worker: {workerType})";

            entityIdToGameObject.Add(entityInfo.SpatialOSEntityId, gameObject);
            linker.LinkGameObjectToSpatialOSEntity(entityInfo.SpatialOSEntityId, gameObject, componentsToAdd);
        }

        public void OnEntityRemoved(EntityId entityId)
        {
            if (!entityIdToGameObject.TryGetValue(entityId, out var go))
            {
                return;
            }

            Object.Destroy(go);
            entityIdToGameObject.Remove(entityId);
        }
    }
}
