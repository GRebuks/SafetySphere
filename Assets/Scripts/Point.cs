using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[JsonConverter(typeof(PointConverter))]
[System.Serializable]
public class Point
{
    public string title;
    public string description;
    public float x;
    public float y;
    public float z;
    public float yRotation;
    public string image;

    public Point()
    {
        title = "";
        description = "";
        x = 0;
        y = 0;
        z = 0;
        yRotation = 0;
        image = "";
    }
}
