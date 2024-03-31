using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int collectible = 0;

    [SerializeField] private Text collectiblesText;

    //[SerializeField] private AudioSource collectSoundEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            //collectSoundEffect.Play(); 
            Destroy(collision.gameObject);
            collectible++;
            collectiblesText.text = "Collectible: " + collectible;
        }
    }
}
