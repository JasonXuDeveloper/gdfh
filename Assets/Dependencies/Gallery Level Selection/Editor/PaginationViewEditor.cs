using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CustomEditor(typeof(PaginationView), true)]
public class PaginationViewEditor : Editor
{
    private string version = "1.0";
    private SerializedProperty galleryManager, itemsContainer, itemPrefab, layout, reverseOrder, itemsWidth, itemsHeight, spaceBetweenItems, visibleItems, firstSelectedIndex,
        isItemsClickable, hasGalleryManager, totalItemsCount;
    private PaginationView pagination;

    private bool showItemSize = true;

    private void OnEnable()
    {
        pagination = target as PaginationView;

        galleryManager = serializedObject.FindProperty("galleryManager");
        itemsContainer = serializedObject.FindProperty("itemsContainer");
        itemPrefab = serializedObject.FindProperty("itemPrefab");
        layout = serializedObject.FindProperty("layout");
        reverseOrder = serializedObject.FindProperty("reverseOrder");
        itemsWidth = serializedObject.FindProperty("itemsWidth");
        itemsHeight = serializedObject.FindProperty("itemsHeight");
        spaceBetweenItems = serializedObject.FindProperty("spaceBetweenItems");
        visibleItems = serializedObject.FindProperty("visibleItems");
        isItemsClickable = serializedObject.FindProperty("isItemsClickable");
        firstSelectedIndex = serializedObject.FindProperty("firstSelectedIndex");
        hasGalleryManager = serializedObject.FindProperty("hasGalleryManager");
        totalItemsCount = serializedObject.FindProperty("totalItemsCount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        MainSettings();
        FooterInformation();

        serializedObject.ApplyModifiedProperties();
    }
    
    private void MainSettings()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(hasGalleryManager, new GUIContent("Has Gallery Manager"));

        if (pagination.hasGalleryManager)
        {
            EditorGUILayout.PropertyField(galleryManager, new GUIContent("Gallery View"));
        }

        EditorGUILayout.PropertyField(itemsContainer, new GUIContent("Items Container"));
        EditorGUILayout.PropertyField(itemPrefab, new GUIContent("Item Prefab"));
        EditorGUILayout.PropertyField(layout, new GUIContent("Layout"));
        EditorGUILayout.PropertyField(reverseOrder, new GUIContent("Reverse Order"));

        if (!pagination.hasGalleryManager)
        {
            EditorGUILayout.PropertyField(totalItemsCount, new GUIContent("Total Items Count"));
        }

        EditorGUILayout.PropertyField(visibleItems, new GUIContent("Visible Items"));
        
        if (!pagination.hasGalleryManager)
        {
            EditorGUILayout.PropertyField(firstSelectedIndex, new GUIContent("Selected Index"));
        }

        showItemSize = EditorGUILayout.Foldout(showItemSize, new GUIContent("Items Size"), true);
        if (showItemSize)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(itemsWidth, new GUIContent("Width"));
            EditorGUILayout.PropertyField(itemsHeight, new GUIContent("Height"));
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(spaceBetweenItems, new GUIContent("Space Between Items"));
        EditorGUILayout.PropertyField(isItemsClickable, new GUIContent("Is Items Clickable"));
        
        if (GUILayout.Button("Force Refresh"))
        {
            pagination.OnEditorForceRefresh();
            EditorUtility.SetDirty(target);
        }
    }

    private void FooterInformation()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        GUILayout.BeginVertical("HelpBox");

        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.normal.textColor = Color.black;
        style.fontSize = 26;
        style.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("Simple Pagination", style);

        EditorGUILayout.Space();
        style.normal.textColor = Color.gray;
        style.fontSize = 18;
        GUILayout.Label("Version: " + version, style);

        EditorGUILayout.Space();
        style.normal.textColor = Color.gray;
        GUILayout.Label("Author: Hojjat.Reyhane", style);
        GUILayout.EndVertical();
    }

    [MenuItem("GameObject/UI/Pagination View", false)]
    private static void CreateGalleryLevelSelection()
    {
        GameObject parent = null;
        if (Selection.activeGameObject)
        {
            Canvas[] parentCanvases = Selection.activeGameObject.GetComponentsInParent<Canvas>();
            if (parentCanvases != null && parentCanvases.Length > 0)
            {
                parent = Selection.activeGameObject;
            }
        }

        if (parent == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObject = new GameObject("Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.gameObject.AddComponent<GraphicRaycaster>();
                Undo.RegisterCreatedObjectUndo(canvasObject, "Create " + canvasObject.name);
            }

            parent = canvas.gameObject;
        }
        
        GameObject pagination = new GameObject("Pagination View");
        PaginationView paginationView = pagination.AddComponent<PaginationView>();
        pagination.transform.SetParent(parent.transform, false);
        
        GameObject itemsContainer = new GameObject("Items Container");
        itemsContainer.transform.SetParent(pagination.transform, false);
        paginationView.itemsContainer = itemsContainer.transform;

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Gallery Level Selection/Prefabs/Pagination/Toggle Pagination Item 01 - Horizontal.prefab");
        if (prefab) paginationView.itemPrefab = prefab;

        if (!FindObjectOfType<EventSystem>())
        {
            GameObject eventObject = new GameObject("EventSystem", typeof(EventSystem));
            eventObject.AddComponent<StandaloneInputModule>();
            Undo.RegisterCreatedObjectUndo(eventObject, "Create " + eventObject.name);
        }

        Selection.activeGameObject = pagination;
        Undo.RegisterCreatedObjectUndo(pagination, "Create " + pagination.name);
    }

    private Canvas GetTopmostCanvas(GameObject component)
    {
        Canvas[] parentCanvases = component.GetComponentsInParent<Canvas>();
        if (parentCanvases != null && parentCanvases.Length > 0)
        {
            return parentCanvases[parentCanvases.Length - 1];
        }
        return null;
    }
}
