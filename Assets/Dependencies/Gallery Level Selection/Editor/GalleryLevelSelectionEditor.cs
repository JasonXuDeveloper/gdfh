using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[CustomEditor(typeof(GalleryLevelSelectionManager), true)]
public class GalleryLevelSelectionEditor : Editor
{
    private string version = "1.0";
    private SerializedProperty items, autoGenerateItemsCount, itemsWidth, itemsHeight, galleryChild,
        itemsContainer, activeItems, spaceBetweenItems, minScaleDownValue, scrollAxis, reverseOrder, infiniteLoop, activeEditorAutoManaging,
        hasRotationOverZ, maxRotationZ, maxDisplacementPos, normalAnimateSpeed, indicatorType, slider, scrollbar, pagination, autoGenerateAddText, autoGenerateAddImageUI,
        levelTitleFormat, txtLevelTitle, start_level, hasColorTransition, transitionColorStart, transitionColorEnd, horizontalAlignment, verticalAlignment, snapItems,
        freeSwipeColliderSize, navigateToClickedItems, linearScale, hasScaleTransition, hasDisplacementTransition, displacementType, linearColor, linearRotation;
    private bool showMainSettings = true;
    private bool showItemSize = true;
    private bool showAutoGenerate = true;
    private bool showLayoutAndMovement = true;
    private bool showTransitionEffects = true;
    private bool showPageIndicator = true;
    private GalleryLevelSelectionManager gallery;

    private void OnEnable()
    {
        gallery = target as GalleryLevelSelectionManager;

        items = serializedObject.FindProperty("items");
        autoGenerateItemsCount = serializedObject.FindProperty("autoGenerateItemsCount");
        autoGenerateAddImageUI = serializedObject.FindProperty("autoGenerateAddImageUI");
        autoGenerateAddText = serializedObject.FindProperty("autoGenerateAddText");
        itemsWidth = serializedObject.FindProperty("itemsWidth");
        itemsHeight = serializedObject.FindProperty("itemsHeight");
        galleryChild = serializedObject.FindProperty("galleryChild");
        itemsContainer = serializedObject.FindProperty("itemsContainer");
        activeItems = serializedObject.FindProperty("activeItems");
        spaceBetweenItems = serializedObject.FindProperty("spaceBetweenItems");
        minScaleDownValue = serializedObject.FindProperty("minScaleDownValue");
        scrollAxis = serializedObject.FindProperty("scrollAxis");
        reverseOrder = serializedObject.FindProperty("reverseOrder");
        infiniteLoop = serializedObject.FindProperty("infiniteLoop");
        activeEditorAutoManaging = serializedObject.FindProperty("activeEditorAutoManaging");
        hasRotationOverZ = serializedObject.FindProperty("hasRotationOverZ");
        maxRotationZ = serializedObject.FindProperty("maxRotationZ");
        maxDisplacementPos = serializedObject.FindProperty("maxDisplacementPos");
        normalAnimateSpeed = serializedObject.FindProperty("normalAnimateSpeed");
        indicatorType = serializedObject.FindProperty("indicatorType");
        slider = serializedObject.FindProperty("slider");
        scrollbar = serializedObject.FindProperty("scrollbar");
        pagination = serializedObject.FindProperty("pagination");
        levelTitleFormat = serializedObject.FindProperty("levelTitleFormat");
        txtLevelTitle = serializedObject.FindProperty("txtLevelTitle");
        start_level = serializedObject.FindProperty("start_level");
        hasColorTransition = serializedObject.FindProperty("hasColorTransition");
        transitionColorStart = serializedObject.FindProperty("transitionColorStart");
        transitionColorEnd = serializedObject.FindProperty("transitionColorEnd");
        horizontalAlignment = serializedObject.FindProperty("horizontalAlignment");
        verticalAlignment = serializedObject.FindProperty("verticalAlignment");
        freeSwipeColliderSize = serializedObject.FindProperty("freeSwipeColliderSize");
        navigateToClickedItems = serializedObject.FindProperty("navigateToClickedItems");
        linearScale = serializedObject.FindProperty("linearScale");
        hasScaleTransition = serializedObject.FindProperty("hasScaleTransition");
        hasDisplacementTransition = serializedObject.FindProperty("hasDisplacementTransition");
        displacementType = serializedObject.FindProperty("displacementType");
        linearColor = serializedObject.FindProperty("linearColor");
        linearRotation = serializedObject.FindProperty("linearRotation");
        snapItems = serializedObject.FindProperty("snapItems");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        MainSettings();
        LayoutAndMovementSettings();
        TransitionEffects();
        PageIndicatorSettings();
        AutoGeneration();
        FooterInformation();

        serializedObject.ApplyModifiedProperties();
    }

    private void MainSettings()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        showMainSettings = EditorGUILayout.Foldout(showMainSettings, new GUIContent("Main Settings"), true);
        EditorStyles.foldout.fontStyle = FontStyle.Normal;

        if (showMainSettings)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(galleryChild, new GUIContent("Gallery Child"));
            EditorGUILayout.PropertyField(itemsContainer, new GUIContent("Items Container"));

            EditorGUILayout.PropertyField(items, new GUIContent("Items Array"), true);

            showItemSize = EditorGUILayout.Foldout(showItemSize, new GUIContent("Items Size", "The size of each item."), true);
            if (showItemSize)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(itemsWidth, new GUIContent("Width"));
                EditorGUILayout.PropertyField(itemsHeight, new GUIContent("Height"));
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }
    }

    private void LayoutAndMovementSettings()
    {
        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        showLayoutAndMovement = EditorGUILayout.Foldout(showLayoutAndMovement, new GUIContent("Layout And Movement"), true);
        EditorStyles.foldout.fontStyle = FontStyle.Normal;

        if (showLayoutAndMovement)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(activeEditorAutoManaging, new GUIContent("Active in Editor Mode"));
            EditorGUILayout.PropertyField(scrollAxis, new GUIContent("Scroll Axis"));

            if (gallery.scrollAxis == GalleryLevelSelectionManager.ScrollAxis.Horizontal)
            {
                EditorGUILayout.PropertyField(verticalAlignment, new GUIContent("Alignment", "Vertical Alignment of Items."));

            }
            else if (gallery.scrollAxis == GalleryLevelSelectionManager.ScrollAxis.Vertical)
            {
                EditorGUILayout.PropertyField(horizontalAlignment, new GUIContent("Alignment", "Horizontal Alignment of Items."));
            }

            EditorGUILayout.PropertyField(reverseOrder, new GUIContent("Reverse Order"));
            EditorGUILayout.PropertyField(infiniteLoop, new GUIContent("Infinite Loop"));
            EditorGUILayout.PropertyField(activeItems, new GUIContent("Active Items", "The number of items that will always calculate and placed in screen. Set it carefully."));
            EditorGUILayout.PropertyField(spaceBetweenItems, new GUIContent("Space Between Items"));
            EditorGUILayout.PropertyField(start_level, new GUIContent("Start Level"));
            EditorGUILayout.PropertyField(snapItems, new GUIContent("Snap Items"));
            EditorGUILayout.PropertyField(navigateToClickedItems, new GUIContent("Navigate To Clicked Items"));
            EditorGUILayout.PropertyField(normalAnimateSpeed, new GUIContent("Animate Frames"));
            EditorGUILayout.PropertyField(freeSwipeColliderSize, new GUIContent("Custom Swipe Collider Size"));

            EditorGUI.indentLevel--;
        }
    }

    private void TransitionEffects()
    {
        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        showTransitionEffects = EditorGUILayout.Foldout(showTransitionEffects, new GUIContent("Transition Effects"), true);
        EditorStyles.foldout.fontStyle = FontStyle.Normal;

        if (showTransitionEffects)
        {
            EditorGUI.indentLevel++;

            GUILayout.BeginVertical("HelpBox");
            EditorGUILayout.PropertyField(hasDisplacementTransition, new GUIContent("Displacement"));

            if (gallery.hasDisplacementTransition)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(maxDisplacementPos, new GUIContent("Max " + (gallery.scrollAxis == GalleryLevelSelectionManager.ScrollAxis.Horizontal ? "Y" : "X") + " Pos", 
                    (gallery.scrollAxis == GalleryLevelSelectionManager.ScrollAxis.Horizontal ? "Y" : "X") + " position of last active item. Not affecting the center item."));
                EditorGUILayout.PropertyField(displacementType, new GUIContent("Type"));
                EditorGUI.indentLevel--;
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("HelpBox");
            EditorGUILayout.PropertyField(hasScaleTransition, new GUIContent("Scale"));

            if (gallery.hasScaleTransition)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(minScaleDownValue, new GUIContent("Min Scale"));
                EditorGUILayout.PropertyField(linearScale, new GUIContent("Linear"));
                EditorGUI.indentLevel--;
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("HelpBox");
            EditorGUILayout.PropertyField(hasRotationOverZ, new GUIContent("Z Rotation"));

            if (gallery.hasRotationOverZ)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(maxRotationZ, new GUIContent("Max Rotation"));
                EditorGUILayout.PropertyField(linearRotation, new GUIContent("Linear"));
                EditorGUI.indentLevel--;
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("HelpBox");
            EditorGUILayout.PropertyField(hasColorTransition, new GUIContent("Color"));

            if (gallery.hasColorTransition)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(transitionColorStart, new GUIContent("Start"));
                EditorGUILayout.PropertyField(transitionColorEnd, new GUIContent("End"));
                EditorGUILayout.PropertyField(linearColor, new GUIContent("Linear"));
                EditorGUI.indentLevel--;
            }
            GUILayout.EndVertical();

            EditorGUI.indentLevel--;
        }
    }

    private void PageIndicatorSettings()
    {
        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        showPageIndicator = EditorGUILayout.Foldout(showPageIndicator, new GUIContent("Page Indicator Settings"), true);
        EditorStyles.foldout.fontStyle = FontStyle.Normal;

        if (showPageIndicator)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(indicatorType, new GUIContent("Page Indicator"));

            if (gallery.indicatorType == GalleryLevelSelectionManager.IndicatorType.Slider)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(slider, new GUIContent("Slider"));
                EditorGUI.indentLevel--;
            }
            else if (gallery.indicatorType == GalleryLevelSelectionManager.IndicatorType.Scrollbar)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(scrollbar, new GUIContent("Scrollbar"));
                EditorGUI.indentLevel--;
            }
            else if (gallery.indicatorType == GalleryLevelSelectionManager.IndicatorType.Pagination)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(pagination, new GUIContent("Pagination"));
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(levelTitleFormat, new GUIContent("Page Title Format"));

            if (gallery.levelTitleFormat != GalleryLevelSelectionManager.LevelTitleFormat.None)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(txtLevelTitle, new GUIContent("Title Text"));
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }
    }

    private void AutoGeneration()
    {
        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        showAutoGenerate = EditorGUILayout.Foldout(showAutoGenerate, new GUIContent("Auto Generate Items", "You can add auto generated items."), true);
        EditorStyles.foldout.fontStyle = FontStyle.Normal;

        if (showAutoGenerate)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(autoGenerateItemsCount, new GUIContent("count"));
            EditorGUILayout.PropertyField(autoGenerateAddImageUI, new GUIContent("Add UI Image"));
            EditorGUILayout.PropertyField(autoGenerateAddText, new GUIContent("Add Text Number"));

            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUI.indentLevel * 16);
            if (GUILayout.Button("Add Items"))
            {
                gallery.AutoGenerateItems();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUI.indentLevel * 16);
            if (GUILayout.Button("Clear All Items"))
            {
                gallery.ClearAllItems();
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUI.indentLevel--;
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
        GUILayout.Label("Gallery Level View", style);

        EditorGUILayout.Space();
        style.normal.textColor = Color.gray;
        style.fontSize = 18;
        GUILayout.Label("Version: " + version, style);

        EditorGUILayout.Space();
        style.normal.textColor = Color.gray;
        GUILayout.Label("Author: Hojjat.Reyhane", style);
        GUILayout.EndVertical();
    }

    [MenuItem("GameObject/UI/Gallery Level Selection", false)]
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
            if(canvas == null)
            {
                GameObject canvasObject = new GameObject("Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.gameObject.AddComponent<GraphicRaycaster>();
                Undo.RegisterCreatedObjectUndo(canvasObject, "Create " + canvasObject.name);
            }

            parent = canvas.gameObject;
        }
        
        GameObject galleryView = new GameObject("Gallery Level Selection");
        RectMask2D mask = galleryView.AddComponent<RectMask2D>();
        mask.enabled = false;
        galleryView.AddComponent<BoxCollider2D>();
        GalleryLevelSelectionManager galleryManager = galleryView.AddComponent<GalleryLevelSelectionManager>();
        galleryView.GetComponent<RectTransform>().sizeDelta = new Vector2(galleryManager.itemsWidth, galleryManager.itemsHeight);
        galleryView.transform.SetParent(parent.transform, false);

        GameObject child = new GameObject("Child");
        child.transform.SetParent(galleryView.transform, false);
        galleryManager.galleryChild = child.transform;

        GameObject itemsContainer = new GameObject("Items Container");
        itemsContainer.transform.SetParent(child.transform, false);
        galleryManager.itemsContainer = itemsContainer.transform;

        galleryManager.autoGenerateItemsCount = 5;
        galleryManager.AutoGenerateItems();

        if (!FindObjectOfType<EventSystem>())
        {
            GameObject eventObject = new GameObject("EventSystem", typeof(EventSystem));
            eventObject.AddComponent<StandaloneInputModule>();
            Undo.RegisterCreatedObjectUndo(eventObject, "Create " + eventObject.name);
        }

        Selection.activeGameObject = galleryView;
        Undo.RegisterCreatedObjectUndo(galleryView, "Create " + galleryView.name);
    }
}
