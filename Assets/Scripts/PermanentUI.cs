using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUI : MonoBehaviour
{
    public int coins = 0;
    public Text coinsNum;
    public static PermanentUI perm;
    private void Start()
    {
        //DontDestroyOnLoad(gameObject);
        if (!perm)
        {
            perm = this;
        }
        else Destroy(gameObject);
    }
    public void Reset()
    {
        coins = 0;
        coinsNum.text = coins.ToString();
    }
}
