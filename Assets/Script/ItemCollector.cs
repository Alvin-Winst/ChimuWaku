using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{

    [SerializeField] private Text collectiblesQty;
    [SerializeField] private Text collectedQty;
    [SerializeField] private Text score;

    //[SerializeField] private AudioSource collectSoundEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            //collectSoundEffect.Play(); 
            Destroy(collision.gameObject);
            int collectibles = Int32.Parse(collectiblesQty.text) +1;
            int scores = (int)Math.Round((float)((float)collectibles / 7 * 100));
            Debug.Log(scores);
            collectiblesQty.text = collectibles.ToString();
            collectedQty.text = collectibles + "/7";
            score.text = scores.ToString() + "/100";
        }
    }
}
