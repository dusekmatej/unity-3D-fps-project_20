using System.Collections;
using Unity.UI;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class statDecrease : MonoBehaviour
{

    public int decreasePercent = 100;
    public TMP_Text decreaseText;
    public Image decreaseImage;
    
    public int decreaseAmount = 1;
    public float decreaseTime = 6;
    public bool isDecreasing = true;
    private float _timer;

    
    void Start()
    {
        StartCoroutine(Decrease());
    }
    
    IEnumerator Decrease()
    {
        while (isDecreasing)
        {
            yield return new WaitForSeconds(decreaseTime);
            decreasePercent = decreasePercent - decreaseAmount;

            decreaseText.text = decreasePercent + "%";
            decreaseImage.fillAmount = 0.5f;
        }
    }
}
