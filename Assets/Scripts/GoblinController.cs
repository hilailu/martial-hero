using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : Enemy
{
    private enum State { idle, run, jump, fall, takehit, attack}
    private State goblinState = State.idle;
    [SerializeField] private float leftpoint;
    [SerializeField] private float rightpoint;
    [SerializeField] private float speed; 
    [SerializeField] private LayerMask land;
    [SerializeField] private AudioSource hit;
    private Collider2D col;
    private Animation anima;
    public int hitCount = 0;
    private bool facingRight = true;

    protected override void Start()
    {
        base.Start();
        col = GetComponent<Collider2D>();
        //base.anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    private void Update()
    {
        if (!base.anim.GetBool("Attacked"))
        {
            Move();
            anim.SetInteger("state", (int)goblinState);
        }
        else
        {
            base.rb.velocity = new Vector2(0, 0);
            Invoke("NotAttacked", 0.417f);
            if (hitCount>=3)
            {
                base.JumpedOn();
                //base.anim.SetTrigger("Death");
            }
        }
    }
    private void Move()
    {
        if (!facingRight)
        {
            if (transform.position.x > leftpoint)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                    
                }
                if (col.IsTouchingLayers(land))
                {
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                }
            }
            else
            {
                facingRight = true;
            }
        }
        else
        {
            if (transform.position.x < rightpoint)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);

                }
                if (col.IsTouchingLayers(land))
                {
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                }
            }
            else
            {
                facingRight = false;
            }
        }
        goblinState = State.run;
    }
    public void Hit()
    {
        hit.Play();
    }
    private void NotAttacked()
    {
        base.anim.SetBool("Attacked", false);
    }

}
