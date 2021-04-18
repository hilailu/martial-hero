using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueNext : MonoBehaviour
{
    public KeyCode _Key;
    public KeyCode _AltKey;
    public Button _button;
    public AudioSource auds;
    public Animator DBAnimator;

    void Awake()
    {
        _button = GetComponent<Button>();
        auds = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(_Key))
        {
            _button.onClick.Invoke();
        }
    }
    public void SoundPlay()
    {
        if (DBAnimator.GetBool("DBOpen"))
        {
            auds.Play();
        }
    }
}
