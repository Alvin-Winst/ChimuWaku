using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WakuLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private AudioSource BGM;

    //[SerializeField] private AudioSource deathSoundEffect;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        //BGM = GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
            Debug.Log("died");
        }
    }

    private void Die()
    {
        //deathSoundEffect.Play();
        //BGM.Stop();
        rb.bodyType = RigidbodyType2D.Static;
        //Debug.Log("Stopped BGM");
        anim.SetTrigger("death");
    }
    private void RestartLevel()
    {
        Debug.Log("Restarted");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
