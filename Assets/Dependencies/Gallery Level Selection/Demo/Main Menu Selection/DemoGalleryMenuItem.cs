using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoGalleryMenuItem : GalleryLevelView {

    public GameObject contentToShow;
    public Image contentMask;

    private bool isContentVisible = false;
    
    public override void UpdateProgress(float value, float linearValue)
    {
        base.UpdateProgress(value, linearValue);

        Color color = Color.white;
        if(contentMask) color = contentMask.color;

        if (linearValue > 0.6f)
        {
            if(isContentVisible && contentToShow)
            {
                color.a = 1;
                contentToShow.SetActive(false);
                isContentVisible = false;
            }
        }
        else if(linearValue <= 0.4f)
        {
            if (!isContentVisible && contentToShow)
            {
                contentToShow.SetActive(true);
                isContentVisible = true;
            }
            
            color.a = linearValue / 0.4f;
        }

        if (contentMask) contentMask.color = color;
        
    }

}
