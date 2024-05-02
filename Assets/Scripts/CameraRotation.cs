using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float rotationValue = 15f;
    public GameObject xrOrigin;

    // Start is called before the first frame update
    void Start()
    {
        xrOrigin = Camera.main.transform.parent.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // On right arrow key press
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            xrOrigin.transform.Rotate(0, rotationValue, 0);
        }
        // On left arrow key press
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            xrOrigin.transform.Rotate(0, -rotationValue, 0);
        }

        // On xr dpad right press
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            xrOrigin.transform.Rotate(0, rotationValue, 0);
        }
        // On xr dpad left press
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            xrOrigin.transform.Rotate(0, -rotationValue, 0);
        }

    }
}
