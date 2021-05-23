using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed = 2;

    [SerializeField] private bool isVertical;
    private bool isMovingRight, isMovingTop;

    [SerializeField] private int horizontalLeft, horizontalRight;
    [SerializeField] private int verticalBot, verticalTop;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isVertical)
        {
            if (!isMovingRight)
            {
                if (transform.position.x > horizontalLeft)
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                else
                    isMovingRight = true;
            }
            else
            {
                if (transform.position.x < horizontalRight)
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                else
                    isMovingRight = false;
            }
        }
        else
        {
            if (!isMovingTop)
            {
                if (transform.position.y > verticalBot)
                    rb.velocity = new Vector2(rb.velocity.x, -speed);
                else
                    isMovingTop = true;
            }
            else
            {
                if (transform.position.y < verticalTop)
                    rb.velocity = new Vector2(rb.velocity.x, speed);
                else 
                    isMovingTop = false;
            }
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
    }*/
}
