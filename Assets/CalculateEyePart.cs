using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateEyePart : MonoBehaviour
{
    public Canvas canvas;
    public Image leftCrosshairImage, rightCrosshairImage;
    // Start is called before the first frame update
    void Start()
    {
        // Get the position of the right controller
        Vector3 rightControllerPosition = GameObject.Find("RightController").transform.position;
        // Center of the sphere
        Vector3 center = Vector3.zero;

        // Radius of the sphere
        float radius = 3f;

        // Direction vector from center to point inside the sphere
        Vector3 directionVector = rightControllerPosition - center;

        // Normalize the direction vector to obtain unit vector
        Vector3 unitVector = directionVector.normalized;

        // Scale unit vector by the radius to get displacement vector
        Vector3 displacementVector = radius * unitVector;

        // Coordinates of the point on the surface of the sphere
        Vector3 pointOnSurface = center + displacementVector;

        // Print coordinates of the point on the surface of the sphere
        Debug.Log("Coordinates of the point on the surface of the sphere:");
        Debug.Log($"X: {pointOnSurface.x}, Y: {pointOnSurface.y}, Z: {pointOnSurface.z}");
    }
}
