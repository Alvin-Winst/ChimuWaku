using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAppearance : MonoBehaviour
{

    public GameObject Option_1;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(HideAndShow(2.0f));
    }

    IEnumerator HideAndShow(float delay)
    {
        Option_1.SetActive(false);
        yield return new WaitForSeconds(delay);
        Option_1.SetActive(true);
    }
}