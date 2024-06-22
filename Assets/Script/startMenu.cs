using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void toControls() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame(Component confirmPopup)
    {
        confirmPopup.gameObject.SetActive(true);
    }

    public void confirmQuit()
    {
        Application.Quit();
    }

    public void cancelQuit(Component confirmPopup)
    {
        confirmPopup.gameObject.SetActive(false);
    }

}
