using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class statDecrease : MonoBehaviour
{

    public int decreasePercent = 100;
    public int decreaseAmount = 1;
    public float decreaseTime = 6;
    public TextMeshProUGUI decreaseText;
    public bool isDecreasing = true;

    public string customkey;
    
    void Start()
    {
        StartCoroutine(Decrease());
    }

    void Update()
    {
        if (Input.GetKeyDown(customkey))
        {
            decreasePercent = decreasePercent + 10;
        }

        decreaseText.text = decreasePercent + "%";
    }

    IEnumerator Decrease()
    {
        while (isDecreasing)
        {
            yield return new WaitForSeconds(decreaseTime);
            decreasePercent = decreasePercent - decreaseAmount;

        }
    }
}
