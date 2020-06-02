using System;
using Improbable.Gdk.Core.Representation.Types;
using UnityEditor;
using UnityEditor.ShortcutManagement;
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
            listProperty = serializedObject.FindProperty(nameof(EntityLinkerDatabase.entityRepresentationResolvers));
        }

        public override VisualElement CreateInspectorGUI()
        {
            // TODO Replace with UXML
            var container = new VisualElement();
            var addButton = CreateNewEntityButton();
            container.Add(addButton);

            // TODO Allow for the list to be "refreshed"
            var listContainer = new PropertyField(listProperty);
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
                        TargetDatabase.entityRepresentationResolvers.Add(instance);
                    }, type);
                }

                menu.ShowAsContext();
            };

            return addButton;
        }
    }
}
