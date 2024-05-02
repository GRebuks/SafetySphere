using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusOnCamera : MonoBehaviour
{
    public bool enableZRotation = false;
    // Update is called once per frame
    void Update()
    {
        // Look at the main camera on x and y axes only.
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
        // Flip the object so that it faces the camera.
        transform.Rotate(0, 180, 0);
        // Set Z rotation to 0.
        if(!enableZRotation)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
    }
}
