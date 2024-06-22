using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endScene : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool levelCompleted = false;

    [SerializeField] private GameObject scorePopup;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Playerr") && !levelCompleted)
        {
            levelCompleted = true;
            rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        scorePopup.SetActive(true);
    }
}
