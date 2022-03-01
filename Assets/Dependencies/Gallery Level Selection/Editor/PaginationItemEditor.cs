using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PaginationItem), true)]
public class PaginationItemEditor : Editor
{
    private SerializedProperty textType, first, second, normal, selected, secondToLast, last, txtFirst, txtSecond, textNormal, textSelected, txtSecondToLast, txtLast;

    private PaginationItem item;

    private void OnEnable()
    {
        item = target as PaginationItem;

        textType = serializedObject.FindProperty("textType");
        first = serializedObject.FindProperty("first");
        second = serializedObject.FindProperty("second");
        normal = serializedObject.FindProperty("normal");
        selected = serializedObject.FindProperty("selected");
        secondToLast = serializedObject.FindProperty("secondToLast");
        last = serializedObject.FindProperty("last");
        txtFirst = serializedObject.FindProperty("txtFirst");
        txtSecond = serializedObject.FindProperty("txtSecond");
        textNormal = serializedObject.FindProperty("textNormal");
        textSelected = serializedObject.FindProperty("textSelected");
        txtSecondToLast = serializedObject.FindProperty("txtSecondToLast");
        txtLast = serializedObject.FindProperty("txtLast");

    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.fontStyle = FontStyle.Bold;
        GUILayout.Label("Styles:", style);
        
        EditorGUI.indentLevel++;
        
        EditorGUILayout.PropertyField(first, new GUIContent("First"));
        EditorGUILayout.PropertyField(second, new GUIContent("Second"));
        EditorGUILayout.PropertyField(normal, new GUIContent("Normal"));
        EditorGUILayout.PropertyField(selected, new GUIContent("Selected"));
        EditorGUILayout.PropertyField(secondToLast, new GUIContent("Second To Last"));
        EditorGUILayout.PropertyField(last, new GUIContent("Last"));

        EditorGUI.indentLevel--;
        EditorGUILayout.PropertyField(textType, new GUIContent("Text Type"));

        if (item.textType != PaginationItem.TextType.None)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(txtFirst, new GUIContent("First Text"));
            EditorGUILayout.PropertyField(txtSecond, new GUIContent("Second Text"));
            EditorGUILayout.PropertyField(textNormal, new GUIContent("Normal Text"));
            EditorGUILayout.PropertyField(textSelected, new GUIContent("Selected Text"));
            EditorGUILayout.PropertyField(txtSecondToLast, new GUIContent("Second To Last Text"));
            EditorGUILayout.PropertyField(txtLast, new GUIContent("Last Text"));
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
