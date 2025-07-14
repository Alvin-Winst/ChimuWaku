using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endScene : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool levelCompleted = false;
    private int playerCount = 0;

    [SerializeField] private GameObject scorePopup;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerCount < 0)
        {
            playerCount = 0;
        }

        if (collision.gameObject.CompareTag("Playerr"))
        {
            playerCount++;
        }
        if (playerCount == 2 && !levelCompleted)
        {
            levelCompleted = true;
            rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
            CompleteLevel();
        }
        Debug.Log(playerCount);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerCount--;
        Debug.Log(playerCount);
    }

    private void CompleteLevel()
    {
        scorePopup.SetActive(true);
    }
}
