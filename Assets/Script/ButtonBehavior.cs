using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{

    public GameObject gate1;
    public GameObject gate2;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Playerr"))
        {
            gate1.SetActive(false);
            gate2.SetActive(false);
        }
        else
        {
            gate1.SetActive(true);
            gate2.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Playerr"))
        {
            gate1.SetActive(true);
            gate2.SetActive(true);
        }
    }

}
