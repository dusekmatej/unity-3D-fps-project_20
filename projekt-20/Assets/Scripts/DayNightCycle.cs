using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    public Transform Light;

    public float xRotation = 0f;
    public float speed = 0.5f;

    void Update()
    {
        Light.Rotate(Vector3.right * speed * Time.deltaTime);
    }
}
