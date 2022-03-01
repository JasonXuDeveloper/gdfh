using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NumberGalleryLevelView : GalleryLevelView
{
    private void Start()
    {
        if (Application.isPlaying)
        {
            GenerateTitle();
        }
    }
    
    private void Update()
    {
#if UNITY_EDITOR

        if (!Application.isPlaying)
        {
            GenerateTitle();
        }
#endif
    }

    private void GenerateTitle()
    {
        text.text = (index + 1) + "";

        if (index > 100) return;
        
        levelName = "";

        int rem = (int)(index + 1) % 10;
        int div = (int)(index + 1) / 10;
        switch (rem)
        {
            case 0:
                if (div == 0)
                    levelName = "Zero";
                else if (div == 1) levelName = "Ten";
                break;
            case 1:
                if (div != 1)
                    levelName = "One";
                else levelName = "Eleven";
                break;
            case 2:
                if (div != 1)
                    levelName = "Two";
                else levelName = "Twelve";
                break;
            case 3:
                if (div != 1)
                    levelName = "Three";
                else levelName = "Thirteen";
                break;
            case 4:
                if (div != 1)
                    levelName = "Four";
                else levelName = "Fourteen";
                break;
            case 5:
                if (div != 1)
                    levelName = "Five";
                else levelName = "Fifteen";
                break;
            case 6:
                if (div != 1)
                    levelName = "Six";
                else levelName = "Sixteen";
                break;
            case 7:
                if (div != 1)
                    levelName = "Seven";
                else levelName = "Seventeen";
                break;
            case 8:
                if (div != 1)
                    levelName = "Eight";
                else levelName = "Eighteen";
                break;
            case 9:
                if (div != 1)
                    levelName = "Nine";
                else levelName = "Nineteen";
                break;
        }

        if (rem != 0 && div >= 2) levelName = " " + levelName;

        switch (div)
        {
            case 2:
                levelName = "Twenty" + levelName;
                break;
            case 3:
                levelName = "Thirty" + levelName;
                break;
            case 4:
                levelName = "forty" + levelName;
                break;
            case 5:
                levelName = "Fifty" + levelName;
                break;
            case 6:
                levelName = "Sixty" + levelName;
                break;
            case 7:
                levelName = "Seventy" + levelName;
                break;
            case 8:
                levelName = "Eighty" + levelName;
                break;
            case 9:
                levelName = "Ninety" + levelName;
                break;
            case 10:
                levelName = "Hundred" + levelName;
                break;
        }
    }

}
