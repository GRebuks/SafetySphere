using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipRotation : MonoBehaviour
{
    public bool xAxis = false;
    public bool yAxis = false;
    public bool zAxis = false;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(xAxis ? 180 : 0, yAxis ? 180 : 0, zAxis ? 180 : 0);
    }
}
