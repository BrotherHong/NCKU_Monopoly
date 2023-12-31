using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour, IPointerClickHandler
{
    public int destinationSceneIndex = 0;
    public bool exitMode = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (exitMode)
        {
            Application.Quit();
            return;
        }
        SceneManager.LoadScene(destinationSceneIndex);
    }

    public void GameOver_MainMenu_Click()
    {
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene("MenuScene");
    }

}
