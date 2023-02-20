using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Sun_Package
{
    [CustomEditor(typeof(MeshRenderer))] 
    [CanEditMultipleObjects]
    public class MeshRendererSortingLayersEditor : Editor
    {
        //
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            var sortingLayerID = serializedObject.FindProperty("m_SortingLayerID");
            var sortingOrder = serializedObject.FindProperty("m_SortingOrder");

            var firstHoriz = EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            EditorGUI.BeginProperty(firstHoriz, GUIContent.none, sortingLayerID);

            var layerNames = GetSortingLayerNames();
            var layerID = GetSortingLayerUniqueIDs();

            var selected = -1;
            var sID = sortingLayerID.intValue;
            for (var i = 0; i < layerID.Length; i++)
            {
                if (sID == layerID[i])
                {
                    selected = i;
                }
            }

            if (selected == -1)
            {
                for (var i = 0; i < layerID.Length; i++)
                {
                    if (layerID[i] == 0)
                    {
                        selected = i;
                    }
                }
            }

            selected = EditorGUILayout.Popup("Sorting Layer", selected, layerNames);

            sortingLayerID.intValue = layerID[selected];

            EditorGUI.EndProperty();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(sortingOrder, new GUIContent("Order in Layer"));

            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }

        //
        public static string[] GetSortingLayerNames()
        {
            var internalEditorUtilityType = typeof(InternalEditorUtility);
            var sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[]) sortingLayersProperty?.GetValue(null, new object[0]);
        }

        //
        public static int[] GetSortingLayerUniqueIDs()
        {
            var internalEditorUtilityType = typeof(InternalEditorUtility);
            var sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
            return (int[]) sortingLayerUniqueIDsProperty?.GetValue(null, new object[0]);
        }
    }
}