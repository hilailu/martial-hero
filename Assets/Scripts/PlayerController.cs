using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

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
    public float jumpForce = 20f;

    public int health = 3;
    public TMP_Text healthNum;
    [SerializeField] private float hitForce = 7f;
    [SerializeField] private AudioSource steps;
    [SerializeField] private AudioSource swoosh;
    [SerializeField] private AudioSource heart;
    [SerializeField] private AudioSource hurt;
    [SerializeField] private Canvas pausedCanvas;
    [SerializeField] private GameObject firstSelectedButton;
    [SerializeField] private GameObject selectedStore;


    private enum State { idle, run, jump, fall, takehit, attack }
    private State state = State.idle;
    private State prevState = State.idle;

    private void Start() 
    {
        if (PlayerPrefs.GetString("volume") == "off") AudioListener.volume = 0f; else AudioListener.volume = 1f;
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
            CheckState();

            if (Input.GetButtonDown("Fire2"))
            {
                Time.timeScale = 0;
                PlayerPrefs.SetInt("paused", 1);
                pausedCanvas.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstSelectedButton);
            }

            if (Input.GetButtonDown("Fire3") && !DiaBoxAnimator.GetBool("DBOpen"))
            {
                if (StoreManager.instance != null && !StoreManager.instance.store.GetBool("Open"))
                {
                    StoreManager.instance.store.SetBool("Open", true);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(selectedStore);
                }
                else if (StoreManager.instance != null && StoreManager.instance.store.GetBool("Open"))
                {
                    StoreManager.instance.store.SetBool("Open", false);
                    StoreManager.instance.willYouBuy.SetBool("Open", false);
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }

        }
        else if (Input.GetButtonDown("Fire2") && PlayerPrefs.GetInt("paused") == 1) ContinueGame();
    }

    private void CheckState()
    {
        if (StoreManager.instance == null || !StoreManager.instance.store.GetBool("Open"))
        {
            if (state != State.takehit)
            {
                if (Input.GetAxis("Horizontal") > 0f)
                    transform.localScale = new Vector2(1, 1);

                if (Input.GetAxis("Horizontal") < 0f)
                    transform.localScale = new Vector2(-1, 1);

                transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Time.deltaTime * speed;

                if (col.IsTouchingLayers(land))
                {
                    if (Input.GetAxis("Horizontal") != 0f && CorCount == 0 && state != State.jump)
                        state = State.run;

                    if (state == State.fall)
                        state = State.idle;

                    if (Input.GetButtonDown("Jump") && !DiaBoxAnimator.GetBool("DBOpen"))
                    {
                        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                        if (state != State.attack)
                            state = State.jump;
                    }
                }

                if (state != State.attack)
                {
                    if (col.IsTouchingLayers(land) && Input.GetAxis("Horizontal") == 0f && state != State.jump)
                        state = State.idle;

                    if (!col.IsTouchingLayers(land) && rb.velocity.y < .1f)
                        state = State.fall;

                    if (Input.GetButtonDown("Fire1"))
                    {
                        prevState = state;
                        state = State.attack;
                        Attack();
                    }
                }
            }

            if (state == State.takehit)
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
        }

        if (StoreManager.instance != null && StoreManager.instance.store.GetBool("Open"))
            state = State.idle;

        anim.SetInteger("state", (int)state);
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
            case "Sign":
                NotifAnimator.SetInteger("SignNum", collision.GetComponent<Sign>().num);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            GoblinController goblin = other.gameObject.GetComponent<GoblinController>();
            if (state == State.attack)
            {
                if (goblin != null)
                {
                    goblin.hitCount++;
                    enemy.GotHit();
                }
                else
                    enemy.JumpedOn();
                 //StartCoroutine(EnemyDeath(other));
            }
            else if (state == State.fall)
            {
                enemy.JumpedOn();
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
            else
            {
                state = State.takehit;                
                hurt.Play();
                ChangeHealth();
                anim.SetInteger("state", (int)state);
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

        if (other.gameObject.CompareTag("Spikes"))
        {
            Vector3 hit = other.contacts[0].normal;
            Debug.Log(hit);
            float angle = Vector3.Angle(hit, Vector3.up);

            state = State.takehit;
            hurt.Play();
            ChangeHealth();
            anim.SetInteger("state", (int)state);

            if (Mathf.Approximately(angle, 0))
            {
                //Down
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                Debug.Log("Down");
            }
            if (Mathf.Approximately(angle, 180))
            {
                //Up
                Debug.Log("Up");
            }
            if (Mathf.Approximately(angle, 90))
            {
                // Sides
                Vector3 cross = Vector3.Cross(Vector3.forward, hit);
                if (cross.y > 0)
                { // left side of the player
                    rb.velocity = new Vector2(hitForce, rb.velocity.y);
                    Debug.Log("Left");
                }
                else
                { // right side of the player
                    rb.velocity = new Vector2(-hitForce, rb.velocity.y);
                    Debug.Log("Right");
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

    private void Attack()
    {
        boxcol.offset = new Vector2(boxcol.offset.x + 1.7f, boxcol.offset.y);
        boxcol.size = new Vector2(boxcol.size.x + 3.5f, boxcol.size.y);
        CorCount = 1;
        StartCoroutine("OnCompleteAttackAnimation");
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
