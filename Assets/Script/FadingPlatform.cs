using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Playerr"))
        {
            anim.SetTrigger("Fading");
            StartCoroutine(StartFade());
/*
            anim.SetTrigger("Muncul");
            StartCoroutine(StartShow());*/
        }
    }

/*    private void Update()
    {
        if(!transform.gameObject.activeInHierarchy)
        {
        }
    }
*/


    IEnumerator StartFade()
    {
        yield return new WaitForSeconds(0.55f);
        transform.gameObject.SetActive(false);
    }

    IEnumerator StartShow()
    {
        yield return new WaitForSeconds(1.1f);
        transform.gameObject.SetActive(true);
    }

}
