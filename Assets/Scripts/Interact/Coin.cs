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
        StartCoroutine(CoinSound());
        PermanentUI.perm.coins += 1;
        PermanentUI.perm.coinsNum.text = PermanentUI.perm.coins.ToString();
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
