using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using GameManagement.SaveSystem;
using UnityEngine;

public class EnterGame : MonoBehaviour
{
    public void StartOrLoad()
    {
        if (SaveManager.Instance.HasSave())
        {
            
        }
    }
}
