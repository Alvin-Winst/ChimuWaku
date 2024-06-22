using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class loadNextScene : MonoBehaviour
{
    VideoPlayer video;
    // Start is called before the first frame update
    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.Play();
        video.loopPointReached += LoadScene;

        //StartCoroutine(LoadScene());


    }

    void LoadScene(UnityEngine.Video.VideoPlayer vp)
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
