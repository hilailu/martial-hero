using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private AudioSource heart;
    private PlayerController player;

    private void Awake()
    {
        heart = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        player = collision.gameObject.GetComponent<PlayerController>();      
        if (player.health < 3)
        {
            player.health += 1;
            player.healthNum.text = player.health.ToString();
        }
        StartCoroutine(HeartSound());
    }

    IEnumerator HeartSound()
    {
        heart.Play();
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }
}
