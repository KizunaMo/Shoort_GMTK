using System.Linq;
using Framework.CustomAttribute;
using Tool;
using UnityEditor;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ComponentTypeAttribute))]
public class ComponentTypeDrawer : PropertyDrawer
{
    private bool showSearch;
    private string searchText = "";
    private Vector2 scrollPosition;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return showSearch ? EditorGUIUtility.singleLineHeight * 12 : EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "Use [ComponentType] with string.");
            return;
        }

        EditorGUI.BeginProperty(position, label, property);

        var identifyComponent = (property.serializedObject.targetObject as MonoBehaviour);
        var currentComponent = GetCurrentComponent(identifyComponent, property.stringValue);

        Rect buttonRect = position;
        buttonRect.height = EditorGUIUtility.singleLineHeight;
        buttonRect.width -= 20;

        string buttonText;
        if (currentComponent != null)
            buttonText = $"{currentComponent.GetType().Name} (Assigned)";
        else if (!string.IsNullOrEmpty(property.stringValue))
            buttonText = $"{property.stringValue} (Missing)";
        else
            buttonText = "Select Component Type";

        if (GUI.Button(buttonRect, buttonText))
        {
            showSearch = !showSearch;
            searchText = "";
        }

        if (showSearch)
        {
            Rect dropdownRect = position;
            dropdownRect.y += EditorGUIUtility.singleLineHeight + 2;
            dropdownRect.height = EditorGUIUtility.singleLineHeight * 10;

            GUI.Box(dropdownRect, "");

            Rect searchBoxRect = dropdownRect;
            searchBoxRect.height = EditorGUIUtility.singleLineHeight;
            searchBoxRect.x += 5;
            searchBoxRect.width -= 10;
            searchBoxRect.y += 2;
            GUI.SetNextControlName("SearchField");
            searchText = EditorGUI.TextField(searchBoxRect, searchText);

            if (Event.current.type == EventType.Repaint)
            {
                GUI.FocusControl("SearchField");
            }

            Rect listRect = dropdownRect;
            listRect.y = searchBoxRect.y + searchBoxRect.height + 2;
            listRect.height = dropdownRect.height - searchBoxRect.height - 4;
            listRect.x += 5;
            listRect.width -= 10;

            var availableComponents = identifyComponent?.GetComponents<Component>()
                .Where(c => c != null && !typeof(IdentifyComponent).IsAssignableFrom(c.GetType()))
                .Where(c => string.IsNullOrEmpty(searchText) ||
                            c.GetType().Name.ToLower().Contains(searchText.ToLower()))
                .OrderBy(c => c.GetType().Name)
                .ToList();

            Debug.Assert(availableComponents != null, nameof(availableComponents) + " != null");
            Rect viewRect = new Rect(0, 0, listRect.width - 20,
                availableComponents.Count * EditorGUIUtility.singleLineHeight);
            scrollPosition = GUI.BeginScrollView(listRect, scrollPosition, viewRect);

            for (int i = 0; i < availableComponents.Count; i++)
            {
                var component = availableComponents[i];
                Rect itemRect = new Rect(0, i * EditorGUIUtility.singleLineHeight,
                    listRect.width - 20, EditorGUIUtility.singleLineHeight);

                if (GUI.Button(itemRect, component.GetType().Name, EditorStyles.label))
                {
                    property.stringValue = component.GetType().FullName;
                    property.serializedObject.ApplyModifiedProperties();

                    var serializedObject = new SerializedObject(identifyComponent);
                    var cachedComponentProperty = serializedObject.FindProperty("cachedComponent");
                    if (cachedComponentProperty != null)
                    {
                        cachedComponentProperty.objectReferenceValue = component;
                        serializedObject.ApplyModifiedProperties();
                    }

                    showSearch = false;
                    GUI.FocusControl(null);
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
                }
            }

            GUI.EndScrollView();
        }

        EditorGUI.EndProperty();
    }

    private Component GetCurrentComponent(MonoBehaviour targetBehaviour, string componentTypeName)
    {
        if (string.IsNullOrEmpty(componentTypeName) || targetBehaviour == null)
            return null;

        var serializedObject = new SerializedObject(targetBehaviour);
        var cachedComponentProperty = serializedObject.FindProperty("cachedComponent");
        if (cachedComponentProperty != null)
        {
            return cachedComponentProperty.objectReferenceValue as Component;
        }

        return null;
    }
}

#endif