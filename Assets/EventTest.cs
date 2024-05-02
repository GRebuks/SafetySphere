using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // On OpenXR trigger press
        if (Input.GetButtonDown("XRI_Right_TriggerButton"))
        {
            Debug.Log("Trigger pressed");
        }
    }
}
