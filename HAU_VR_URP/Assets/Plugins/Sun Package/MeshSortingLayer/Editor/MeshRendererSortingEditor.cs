using System;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    [CustomEditor(typeof(MeshRenderer))] 
    [CanEditMultipleObjects]
    public class MeshRendererSortingEditor : Editor
    {
        private Editor defaultEditor;
        private MeshRenderer meshRenderer;
        private static bool showSorting = true;
        private string header = "2D Sorting";

        private SerializedProperty sortingLayerIdProperty;
        private SerializedProperty sortingOrderProperty;

        private void OnEnable()
        {
            defaultEditor = CreateEditor(targets, Type.GetType("UnityEditor.MeshRendererEditor, UnityEditor"));
            meshRenderer = target as MeshRenderer;

            sortingLayerIdProperty = serializedObject.FindProperty("m_SortingLayerID");
            sortingOrderProperty = serializedObject.FindProperty("m_SortingOrder");
        }

        private void OnDisable()
        {
            //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
            //Also, make sure to call any required methods like OnDisable
            var disableMethod = defaultEditor.GetType().GetMethod("OnDisable",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (disableMethod != null)
                disableMethod.Invoke(defaultEditor, null);
            DestroyImmediate(defaultEditor);
        }

        public override void OnInspectorGUI()
        {
            defaultEditor.OnInspectorGUI();

            serializedObject.Update();

            showSorting = EditorGUILayout.BeginFoldoutHeaderGroup(showSorting, header);
            if (showSorting)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                var newId = DrawSortingLayersPopup(meshRenderer.sortingLayerID);
                if (EditorGUI.EndChangeCheck())
                {
                    sortingLayerIdProperty.intValue = newId;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                var order = EditorGUILayout.IntField("Sorting Order", meshRenderer.sortingOrder);
                if (EditorGUI.EndChangeCheck())
                {
                    sortingOrderProperty.intValue = order;
                }

                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();
        }

        static int DrawSortingLayersPopup(int layerID)
        {
            var layers = SortingLayer.layers;
            var names = layers.Select(l => l.name).ToArray();
            if (!SortingLayer.IsValid(layerID))
            {
                layerID = layers[0].id;
            }

            var layerValue = SortingLayer.GetLayerValueFromID(layerID);
            var newLayerValue = EditorGUILayout.Popup("Sorting Layer", layerValue, names);
            return layers[newLayerValue].id;
        }
    }
}