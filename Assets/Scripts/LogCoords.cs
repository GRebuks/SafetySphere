using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

[System.Serializable]
public class DataContainer
{
    public List<InfoPoint> InfoPoints { get; set; }
    public List<QuizPoint> QuizPoints { get; set; }
    public List<NavigationPoint> NavigationPoints { get; set; }

    public DataContainer()
    {
        InfoPoints = new List<InfoPoint>();
        QuizPoints = new List<QuizPoint>();
        NavigationPoints = new List<NavigationPoint>();
    }
}

public class LogCoords : MonoBehaviour
{
    public Sprite[] images;

    public GameObject navigationSpheresParent;
    public GameObject navigationSpherePrefab;
    public GameObject infoSpherePrefab;
    public GameObject quizSpherePrefab;


    public string jsonFilePath = "Assets/Resources/positions.json";

    void Start()
    {
        images = Resources.LoadAll<Sprite>("Photos");
        navigationSpheresParent = GameObject.Find("ActionPoints");
        PointController.navigationPointPrefab = Resources.Load<GameObject>("Prefabs/NavigationPointCanvas");
        PointController.infoPointPrefab = Resources.Load<GameObject>("Prefabs/InfoPointCanvas");
        PointController.quizPointPrefab = Resources.Load<GameObject>("Prefabs/QuizPointCanvas");
        PointController.quizAnswerPrefab = Resources.Load<GameObject>("Prefabs/QuizButton");
        PointController.navigationSpheresParent = navigationSpheresParent;
        PointController.images = images;
        PointController.LoadPositionsAndInstantiateSpheres();
        PointController.ChangeInfo("Sveicināti Rīgas Valsts tehnikumā!", ": -)");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pointCoordinates = CalculatePointCoordinates();
            PointController.InstantiatePoint(pointCoordinates);
            PointController.AddNewPosition<NavigationPoint>(pointCoordinates.x, pointCoordinates.y, pointCoordinates.z, PointController.GetCurrentImageName());
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pointCoordinates = CalculatePointCoordinates();
            PointController.InstantiatePoint(pointCoordinates);
            PointController.AddNewPosition<InfoPoint>(pointCoordinates.x, pointCoordinates.y, pointCoordinates.z, PointController.GetCurrentImageName());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            UnityEngine.Debug.Log(Camera.main.transform.rotation.eulerAngles.y);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pointCoordinates = CalculateControllerAimCoordinates();
            PointController.InstantiatePoint(pointCoordinates);
            PointController.AddNewPosition<NavigationPoint>(pointCoordinates.x, pointCoordinates.y, pointCoordinates.z, PointController.GetCurrentImageName());
        }
    }

    private static Vector3 CalculatePointCoordinates()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit))
        {
            print(hit.point);
        }

        float radius = 3;
        Vector3 cameraNavigationPoint = Camera.main.transform.position;
        Vector3 cameraDirection = Camera.main.transform.forward;
        Vector3 sphereCenter = Vector3.zero;
        Vector3 sphereToCamera = cameraNavigationPoint - sphereCenter;
        float a = Vector3.Dot(cameraDirection, cameraDirection);
        float b = 2 * Vector3.Dot(sphereToCamera, cameraDirection);
        float c = Vector3.Dot(sphereToCamera, sphereToCamera) - radius * radius;
        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            print("No real solution");
        }
        else
        {
            float t1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
            float t2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);
            Vector3 solution1 = cameraNavigationPoint + t1 * cameraDirection;
            Vector3 solution2 = cameraNavigationPoint + t2 * cameraDirection;
            print(solution1);
            print(solution2);

            return solution1;
        }
        return new Vector3();
    }
    private static Vector3 CalculateControllerAimCoordinates()
    {
        float radius = 3f;
        // Get the position of the right controller
        Transform rightControllerPosition = GameObject.Find("Right Controller").transform;
        // Center of the sphere
        Vector3 center = Vector3.zero;

        // Normalize the direction vector
        Vector3 forwardDirection = rightControllerPosition.forward;

        // Scale the normalized direction vector by the radius to find point C
        Vector3 pointC = center + (forwardDirection.normalized * radius);

        // Print coordinates of the point on the surface of the sphere
        UnityEngine.Debug.Log("Coordinates of the point on the surface of the sphere:");
        UnityEngine.Debug.Log($"X: {pointC.x}, Y: {pointC.y}, Z: {pointC.z}");
        return pointC;
    }
}
