using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyWaku : MonoBehaviour
{

    double chimuBottom, wakuTop;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Chimu")
        {
            chimuBottom = collision.transform.position.y - collision.collider.bounds.size.y/2;
            wakuTop = transform.position.y;
            if (chimuBottom >= wakuTop - 0.01)
            {
               collision.gameObject.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Chimu")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
