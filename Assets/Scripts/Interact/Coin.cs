using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioSource coin;

    private void Awake()
    {
        coin = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CoinSound());
            Statistics.instance.allCoins++;
            PermanentUI.perm.coins += 1;
            PermanentUI.perm.coinsNum.text = PermanentUI.perm.coins.ToString();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }

    IEnumerator CoinSound()
    {
        coin.Play();
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }
}
