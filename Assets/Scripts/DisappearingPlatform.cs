using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 pos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DisappearRoutine());
        }
    }

    private IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(1f);
        sr.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(2f);
        transform.position = pos;
        sr.enabled = true;
    }
}
