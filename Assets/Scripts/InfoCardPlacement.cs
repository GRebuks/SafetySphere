using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCardPlacement : MonoBehaviour
{
    public Transform playerCenter;
    public float maxRadius = 2f;
    public float cardOffset = 0.5f;

    void Update()
    {
        // Get the controller's position and rotation
        Vector3 controllerPosition = transform.position;
        Quaternion controllerRotation = transform.rotation;

        // Calculate the direction from the player center to the controller
        Vector3 directionToController = controllerPosition - playerCenter.position;

        // Clamp the distance from the player center to the controller
        float distance = Mathf.Min(directionToController.magnitude, maxRadius);
        Vector3 targetPosition = playerCenter.position + directionToController.normalized * distance;

        // Add an offset to the target position
        targetPosition += playerCenter.forward * cardOffset;

        // Set the position of the text card
        transform.position = targetPosition;
    }
}
