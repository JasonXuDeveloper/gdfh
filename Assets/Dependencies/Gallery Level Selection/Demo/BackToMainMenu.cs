using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour {

    public string sceneName;

    public void OnButtonClick()
    {
        SceneManager.LoadScene(sceneName);
    }
}
