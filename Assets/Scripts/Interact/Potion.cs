using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private AudioSource potion;
    private GameObject player;

    private void Awake()
    {
        potion = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        player = collision.gameObject;
        player.GetComponent<PlayerController>().jumpForce += 10f;
        player.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(PotionSound());
        StartCoroutine(ResetPowerUp());
    }

    IEnumerator PotionSound()
    {
        potion.Play();
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }

    IEnumerator ResetPowerUp()
    {
        yield return new WaitForSeconds(10);
        player.GetComponent<PlayerController>().jumpForce -= 10f;
        player.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
