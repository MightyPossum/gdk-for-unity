using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Representation;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectCreation
{
    public static class GameObjectCreationHelper
    {
        private static readonly string WorkerNotCreatedErrorMessage = $"{nameof(EnableStandardGameObjectCreation)} should be called only after a worker has been initialised for the world.";

        public static void EnableStandardGameObjectCreation(World world, EntityLinkerDatabase entityLinkerDatabase, GameObject workerGameObject = null)
        {
            var workerSystem = world.GetExistingSystem<WorkerSystem>();
            if (workerSystem == null)
            {
                throw new InvalidOperationException(WorkerNotCreatedErrorMessage);
            }

            var creator = new GameObjectCreatorFromMetadata(workerSystem.WorkerType, workerSystem.Origin, entityLinkerDatabase);
            EnableStandardGameObjectCreation(world, creator, workerGameObject);
        }

        public static void EnableStandardGameObjectCreation(World world, IEntityGameObjectCreator creator,
            GameObject workerGameObject = null)
        {
            var workerSystem = world.GetExistingSystem<WorkerSystem>();
            if (workerSystem == null)
            {
                throw new InvalidOperationException(WorkerNotCreatedErrorMessage);
            }

            if (world.GetExistingSystem<GameObjectInitializationSystem>() != null)
            {
                workerSystem.LogDispatcher.HandleLog(LogType.Error, new LogEvent(
                        "You should only call EnableStandardGameobjectCreation() once on worker setup")
                    .WithField(LoggingUtils.LoggerName, nameof(GameObjectCreationHelper)));
                return;
            }

            world.AddSystem(new GameObjectInitializationSystem(creator, workerGameObject));
        }
    }
}
