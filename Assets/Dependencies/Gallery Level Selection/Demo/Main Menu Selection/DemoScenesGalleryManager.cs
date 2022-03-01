using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DemoScenesGalleryManager : GalleryLevelSelectionManager {

    public override void OnItemsClicked(int index, bool isCenter)
    {
        base.OnItemsClicked(index, isCenter);

        if (isCenter)
        {
            SceneManager.LoadScene(items[index].levelName);
        }
    }
}
