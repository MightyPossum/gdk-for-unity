using System;
using Improbable.Gdk.Core.Representation.Types;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Improbable.Gdk.Core.Representation.Editor
{
    [CustomEditor(typeof(EntityLinkerDatabase))]
    public class EntityLinkerDatabaseEditor : UnityEditor.Editor
    {
        private static TypeCache.TypeCollection entityRepresentationTypes;

        private EntityLinkerDatabase TargetDatabase => (EntityLinkerDatabase) target;
        private SerializedProperty listProperty;

        static EntityLinkerDatabaseEditor()
        {
            entityRepresentationTypes = TypeCache.GetTypesDerivedFrom<IEntityRepresentation>();
        }

        private void OnEnable()
        {
            listProperty = serializedObject.FindProperty(nameof(EntityLinkerDatabase.EntityRepresentationResolvers));
        }

        public override VisualElement CreateInspectorGUI()
        {
            // TODO Replace with UXML
            var container = new VisualElement();
            var addButton = CreateNewEntityButton();
            container.Add(addButton);

            // TODO Allow for the list to be "refreshed"
            var listContainer = new VisualElement();
            listContainer.Bind(serializedObject);

            for (var i = 0; i < listProperty.arraySize; i++)
            {
                var representation = TargetDatabase.EntityRepresentationResolvers[i];
                var elementTypeName = representation.GetType().Name;
                var serializedProperty = listProperty.GetArrayElementAtIndex(i);

                var entityType = representation.EntityType;

                // TODO Replace with UXML
                var groupElement = new VisualElement();
                var labelElement = new Label { text = $"{entityType} ({elementTypeName})" };
                var propertyElement = new PropertyField(serializedProperty);

                Debug.Log($"{serializedProperty.hasChildren}");

                propertyElement.Add(labelElement);
                groupElement.Add(propertyElement);
                listContainer.Add(groupElement);
            }

            listContainer.style.minHeight = 300;
            //listContainer.Bind(serializedObject);

            container.Add(listContainer);

            return container;
        }


        private Button CreateNewEntityButton()
        {
            var addButton = new Button { text = "Add Entity" };
            addButton.clicked += () =>
            {
                var menu = new GenericMenu();

                foreach (var type in entityRepresentationTypes)
                {
                    menu.AddItem(new GUIContent(type.Name), false, data =>
                    {
                        var instance = (IEntityRepresentation) Activator.CreateInstance((Type) data);
                        TargetDatabase.EntityRepresentationResolvers.Add(instance);
                    }, type);
                }

                menu.ShowAsContext();
            };

            return addButton;
        }
    }

    [CustomPropertyDrawer(typeof(SimpleEntityLink))]
    public class SimpleEntityLinkDrawerUIE : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            var entityType = new PropertyField(property.FindPropertyRelative("entityType"));
            var prefab = new PropertyField(property.FindPropertyRelative("Prefab"));

            container.Add(entityType);
            container.Add(prefab);

            return container;
        }
    }
}
