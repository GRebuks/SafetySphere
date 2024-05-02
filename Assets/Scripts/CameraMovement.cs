using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate camera using mouse input
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);

        // Rotate camera using mouse input on X axis
        transform.Rotate(-Input.GetAxis("Mouse Y"), 0, 0);

        // Set Z axis rotation to 0
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        // On E press hide cursor
        if (Input.GetKeyDown(KeyCode.E))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // On Q press show cursor
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }
}
