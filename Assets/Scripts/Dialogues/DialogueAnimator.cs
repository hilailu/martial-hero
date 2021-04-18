using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimator : MonoBehaviour
{
    public Animator NotifAnimator;
    public DialogueManager dm;

    public void OnTriggerEnter2D(Collider2D other)
    {
        NotifAnimator.SetBool("NOpen", true);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        NotifAnimator.SetBool("NOpen", false);
        dm.EndDialogue();
    }
}
