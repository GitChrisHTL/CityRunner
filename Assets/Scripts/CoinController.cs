using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinController : MonoBehaviour
{
    public int coinCount;
    public Text coinText;
    void Start()
    {

    }

    void Update()
    {
        coinText.text = coinCount.ToString();
    }
}
