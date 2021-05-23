using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall : MonoBehaviour
{
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Statistics.instance.deaths++;
            PermanentUI.perm.Reset();
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().health = 3;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
