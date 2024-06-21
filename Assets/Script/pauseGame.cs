using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseGame : MonoBehaviour
{
    [SerializeField] private Component pausePopup;

    // Update is called once per frame
    private void Update()
    {
        checkInput();
    }

    private void checkInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            pausePopup.gameObject.SetActive(true);
        }
    }

}
