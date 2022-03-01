using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryLevelView : MonoBehaviour
{
    public GalleryLevelSelectionManager manager;
    public Image image;
    public Text text;
    public string levelName;
    public bool colorEffectOnText = false;

    [HideInInspector] public float index;
    [HideInInspector] public float progress;

    public virtual void UpdateProgress(float value, float linearValue)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            progress = -1;
        }
#endif

        if (value != progress)
        {
            if (manager && manager.hasColorTransition)
            {
                if (image) image.color = GetFadedColor(manager.transitionColorStart, manager.transitionColorEnd, manager.linearColor ? linearValue : value);
                if (text && colorEffectOnText) text.color = GetFadedColor(manager.transitionColorStart, manager.transitionColorEnd, manager.linearColor ? linearValue : value);
            }

            progress = value;
        }
    }
    
    public static Color GetFadedColor(Color start, Color end, float ratio)
    {
        return new Color((start.r + (ratio * (end.r - start.r))), (start.g + (ratio * (end.g - start.g))), (start.b + (ratio * (end.b - start.b))), (start.a + (ratio * (end.a - start.a))));
    }
}
