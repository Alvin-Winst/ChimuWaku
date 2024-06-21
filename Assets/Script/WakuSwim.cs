using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakuSwim : MonoBehaviour
{
    [HideInInspector] public bool isInWater;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Water"))
        {
            isInWater = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            isInWater = false;
        }
    }
}
