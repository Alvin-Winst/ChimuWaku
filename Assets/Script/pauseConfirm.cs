using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseConfirm : MonoBehaviour
{
    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void backToGame(Component pausePopup)
    {
        pausePopup.gameObject.SetActive(false);
    }
}
