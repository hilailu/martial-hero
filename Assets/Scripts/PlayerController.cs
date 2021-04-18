using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Animator NotifAnimator;
    public Animator DiaBoxAnimator;
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;
    private BoxCollider2D boxcol;
    private Animation anima;
    private int CorCount = 0;

    [SerializeField] private LayerMask land;
    [SerializeField] private float speed = 9f;
    [SerializeField] private float jumpForce = 20f;

    [SerializeField] private int health = 3;
    [SerializeField] private Text healthNum;
    [SerializeField] private float hitForce = 7f;
    [SerializeField] private AudioSource steps;
    [SerializeField] private AudioSource coin;
    [SerializeField] private AudioSource swoosh;
    [SerializeField] private AudioSource potion;
    [SerializeField] private AudioSource heart;
    [SerializeField] private AudioSource hurt;
    [SerializeField] private Canvas pausedCanvas;
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private Text pauseText, continueButton, toMenuButton;

    private enum State { idle, run, jump, fall, takehit, attack }
    private State state = State.idle;
    private State prevState = State.idle;
    private void Awake()
    {
        switch (PlayerPrefs.GetString("lang"))
        {
            case "ru":
                pauseText.text = "ПАУЗА";
                continueButton.text = "ПРОДОЛЖИТЬ";
                toMenuButton.text = "В ГЛАВНОЕ МЕНЮ";
                break;
            case "eng":
                pauseText.text = "PAUSED";
                continueButton.text = "RESUME";
                toMenuButton.text = "BACK TO MAIN MENU";
                break;
        }
        if (PlayerPrefs.GetString("volume") == "off") AudioListener.volume = 0f; else AudioListener.volume = 1f;
    }
    
    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        boxcol = GetComponent<BoxCollider2D>();
        healthNum.text = health.ToString();
    }
    public void Footstep()
    {
        steps.Play();
    }
    public void Swoosh()
    {
        swoosh.Play();
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("paused") == 0)
        {
            if (state != State.takehit)
            {
                Movement();
            }
            StateSwitch();
            anim.SetInteger("state", (int)state);
            if (Input.GetButtonDown("Fire2"))
            {
                Time.timeScale = 0;
                PlayerPrefs.SetInt("paused", 1);
                pausedCanvas.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstSelectedButton);
            }
        }
        else if (Input.GetButtonDown("Fire2") && PlayerPrefs.GetInt("paused") == 1) ContinueGame();
    }
    public void ContinueGame()
    {
        pausedCanvas.gameObject.SetActive(false);
        PlayerPrefs.SetInt("paused", 0);
        Time.timeScale = 1;
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Collectable":
                coin.Play();
                Destroy(collision.gameObject);
                PermanentUI.perm.coins += 1;
                PermanentUI.perm.coinsNum.text = PermanentUI.perm.coins.ToString();
                break;
            case "Sign1":
                NotifAnimator.SetInteger("SignNum", 1);
                break;
            case "Sign2":
                NotifAnimator.SetInteger("SignNum", 2);
                break;
            case "Sign3":
                NotifAnimator.SetInteger("SignNum", 3);
                break;
            case "Sign4":
                NotifAnimator.SetInteger("SignNum", 4);
                break;
            case "Sign5":
                NotifAnimator.SetInteger("SignNum", 5);
                break;
            case "Sign6":
                NotifAnimator.SetInteger("SignNum", 6);
                break;
            case "Sign7":
                NotifAnimator.SetInteger("SignNum", 7);
                break;
            case "Potion":
                potion.Play();
                Destroy(collision.gameObject);
                jumpForce += 10f;
                GetComponent<SpriteRenderer>().color = Color.red;
                StartCoroutine(ResetPowerUp());
                break;
            case "Heart":
                heart.Play();
                Destroy(collision.gameObject);
                if (health < 3)
                {
                    health += 1;
                    healthNum.text = health.ToString();
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            GoblinController goblin = other.gameObject.GetComponent<GoblinController>();
            if (state == State.attack)
            {
                goblin.hitCount++;
                enemy.GotHit();
                 //StartCoroutine(EnemyDeath(other));
            }
            else if (state == State.fall)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.takehit;
                hurt.Play();
                ChangeHealth();
                if (other.transform.position.x > transform.position.x)
                {
                    //enemy to my right, hero takes damage and thrown to left
                    rb.velocity = new Vector2(-hitForce, rb.velocity.y);
                }
                else
                {
                    //enemy to my left, hero takes damage and thrown to right
                    rb.velocity = new Vector2(hitForce, rb.velocity.y);
                }
            }
        }
    }
    private void ChangeHealth()
    {
        health -= 1;
        healthNum.text = health.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");
        //left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        //right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        //jump
        if (Input.GetButtonDown("Jump") && col.IsTouchingLayers(land) && !DiaBoxAnimator.GetBool("DBOpen"))
        {
            Jump();
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jump;
    }
    private void StateSwitch()
    {
        //fall when jumping
        if (state == State.jump)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.fall;
            }
        }
        //idle when done falling
        else if (state == State.fall)
        {
            if (col.IsTouchingLayers(land))
            {
                state = State.idle;
            }
        }
        //idle when done taking hit or fall when you got hit while jumping
        else if (state == State.takehit)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
            else if (!col.IsTouchingLayers(land) && rb.velocity.y < .1f)
            {
                state = State.fall;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f && CorCount == 0 && col.IsTouchingLayers(land))
        {
            state = State.run;  
            if (Input.GetButtonDown("Fire1"))
            {
                state = State.attack;
                Attack();

            }
        }

        //attacking
        else if (Input.GetButtonDown("Fire1") && state != State.attack)
        {
            prevState = state;
            state = State.attack;
        }
        //attack animation
        else if (state == State.attack && CorCount == 0)
        {
            Attack();
        }
        //any other options being either idling or falling
        else
        {
            if (state != State.attack)
            {
                state = State.idle;
                if (!col.IsTouchingLayers(land) && rb.velocity.y < .1f)
                {
                    state = State.fall;
                }
            }
        }

    }
    private void Attack()
    {
        boxcol.offset = new Vector2(boxcol.offset.x + 1.7f, boxcol.offset.y);
        boxcol.size = new Vector2(boxcol.size.x + 3.5f, boxcol.size.y);
        CorCount = 1;
        StartCoroutine("OnCompleteAttackAnimation");
    }

    IEnumerator ResetPowerUp()
    {
        yield return new WaitForSeconds(10);
        jumpForce -= 10f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator EnemyDeath(Collision2D other)
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(other.gameObject);
    }

    IEnumerator OnCompleteAttackAnimation()
    {
        yield return new WaitForSeconds(0.417f);
        boxcol.offset = new Vector2(boxcol.offset.x - 1.7f, boxcol.offset.y);
        boxcol.size = new Vector2(boxcol.size.x - 3.5f, boxcol.size.y);
        CorCount = 0;
        state = prevState;
    }
}
