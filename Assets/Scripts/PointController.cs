using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System.Linq;

[DefaultExecutionOrder(-100)]
public class PointController : MonoBehaviour
{
	public static Sprite[] images;

	public static GameObject navigationSpheresParent;
	public static GameObject navigationPointPrefab;
	public static GameObject infoPointPrefab;
	public static GameObject quizPointPrefab;
	public static GameObject quizAnswerPrefab;

	public static string jsonFilePath = "Assets/Resources/positions.json";

	private void Awake()
	{
		images = Resources.LoadAll<Sprite>("Photos");
		navigationSpheresParent = GameObject.Find("ActionPoints");
		navigationPointPrefab = Resources.Load<GameObject>("Prefabs/NavigationPointCanvas");
		infoPointPrefab = Resources.Load<GameObject>("Prefabs/InfoPointCanvas");
		quizPointPrefab = Resources.Load<GameObject>("Prefabs/QuizPointCanvas");
		quizAnswerPrefab = Resources.Load<GameObject>("Prefabs/QuizButton");
	}

    public static void NavigationActivate(NavigationPoint navigationPoint)
    {
        NavigateTo(navigationPoint.navigateToImage);
        RotatePlayer(navigationPoint);
        DestroyAllNavigationSpheres();
        LoadPositionsAndInstantiateSpheres();
    }

    private static void RotatePlayer(NavigationPoint navigationPoint)
    {
        GameObject player = GameObject.Find("XR Origin (XR Rig)");
        if (player != null)
        {
            float cameraYRotation = Mathf.Repeat(Camera.main.transform.rotation.eulerAngles.y, 360f);
            float targetYRotationNormalized = Mathf.Repeat(navigationPoint.yRotation, 360f);

            float deltaRotation = targetYRotationNormalized - (0 + cameraYRotation);

            if (deltaRotation > 180f)
                deltaRotation -= 360f;
            else if (deltaRotation < -180f)
                deltaRotation += 360f;

            player.transform.Rotate(0f, deltaRotation, 0f, Space.World);
        }
    }

    public static void LoadPositionsAndInstantiateSpheres()
	{
		string jsonString = File.ReadAllText(jsonFilePath);
        try
        {
            PointsContainer<Point> pointData = JsonConvert.DeserializeObject<PointsContainer<Point>>(jsonString, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new PointConverter() }
            });
            UnityEngine.Debug.Log("Loaded positions from JSON file");
            UnityEngine.Debug.Log(pointData);

            //var navigationPoints = pointData.points.OfType<NavigationPoint>().ToList();
            //var infoPoints = pointData.points.OfType<InfoPoint>().ToList();
            //var quizPoints = pointData.points.OfType<QuizPoint>().ToList();

            //foreach (NavigationPoint navigationPoint in navigationPoints)
            //{
            //    if (navigationPoint.image == GetCurrentImageName())
            //    {
            //        InstantiatePoint(navigationPoint);
            //    }
            //}

            //foreach (InfoPoint infoPoint in infoPoints)
            //{
            //    if (infoPoint.image == GetCurrentImageName())
            //    {
            //        InstantiatePoint(infoPoint);
            //    }
            //}

            //foreach (QuizPoint quizPoint in quizPoints)
            //{
            //    if (quizPoint.image == GetCurrentImageName())
            //    {
            //        InstantiatePoint(quizPoint);
            //    }
            //}
        } catch (JsonReaderException e)
        {
            UnityEngine.Debug.LogError($"Error reading JSON file: {e.Message}");
        }
    }

    public static void InstantiatePoint(Point position)
    {
        GameObject sphere;
        string objectName;

        if (position is NavigationPoint navigationPoint)
        {
            sphere = Instantiate(navigationPointPrefab, new Vector3(navigationPoint.x, navigationPoint.y, navigationPoint.z), Quaternion.identity);
            objectName = $"{navigationPoint.title}-{navigationPoint.navigateToImage}";
            NavigationObjectController objectController = sphere.AddComponent<NavigationObjectController>();
            objectController.objectInfo = navigationPoint;
        }
        else if (position is InfoPoint infoPoint)
        {
            sphere = Instantiate(infoPointPrefab, new Vector3(infoPoint.x, infoPoint.y, infoPoint.z), Quaternion.identity);
            objectName = $"{infoPoint.title}-info";
            InfoObjectController objectController = sphere.AddComponent<InfoObjectController>();
            objectController.objectInfo = infoPoint;
        }
        else if (position is QuizPoint quizPoint)
        {
            sphere = Instantiate(quizPointPrefab, new Vector3(quizPoint.x, quizPoint.y, quizPoint.z), Quaternion.identity);
            objectName = $"{quizPoint.title}-quiz";
            QuizObjectController objectController = sphere.AddComponent<QuizObjectController>();
            objectController.objectInfo = quizPoint;

            if (!quizPoint.enabled)
            {
                ColorBlock colors = sphere.GetComponent<Button>().colors;
                colors.normalColor = quizPoint.isCorrect ? Color.green : Color.red;
                colors.highlightedColor = colors.normalColor;
                colors.pressedColor = colors.normalColor;
                colors.selectedColor = colors.normalColor;
                sphere.GetComponent<Button>().colors = colors;
            }
        }
        else
        {
            sphere = Instantiate(navigationPointPrefab, new Vector3(position.x, position.y, position.z), Quaternion.identity);
            objectName = position.title;
        }

        sphere.transform.SetParent(navigationSpheresParent.transform);
        sphere.name = objectName;
    }

    public static void InstantiatePoint(Vector3 positionVector)
	{
		GameObject sphere = Instantiate(navigationPointPrefab, positionVector, Quaternion.identity);
		sphere.transform.SetParent(navigationSpheresParent.transform);
	}

	public static string GetCurrentImageName()
	{
		GameObject imageSphere = GameObject.Find("ImageSphere");
		if (imageSphere != null)
		{
			Renderer sphereRenderer = imageSphere.GetComponent<Renderer>();
			if (sphereRenderer != null && sphereRenderer.material != null && sphereRenderer.material.mainTexture != null)
			{
				return sphereRenderer.material.mainTexture.name;
			}
		}
		return "DefaultImageName";
	}

    public static void AddNewPosition<T>(float newX, float newY, float newZ, string newImage) where T : Point, new()
    {
        string jsonString = File.ReadAllText(jsonFilePath);
        PointsContainer<T> pointData = JsonConvert.DeserializeObject<PointsContainer<T>>(jsonString);

        T newPosition = new T()
        {
            x = newX,
            y = newY,
            z = newZ,
            yRotation = 0f,
            title = "Title",
            description = "Description",
            image = newImage,
        };

        if (pointData == null)
        {
            pointData = new PointsContainer<T>();
        }

        if (pointData.points == null)
        {
            pointData.points = new List<T>();
        }

        pointData.points.Add(newPosition);

        SaveAllPositions(pointData);
    }

    public static void ChangeImage()
	{
		GameObject largeSphere = GameObject.Find("ImageSphere");
		Renderer sphereRenderer = largeSphere.GetComponent<Renderer>();
		Sprite image = images[UnityEngine.Random.Range(0, images.Length)];
		sphereRenderer.material.SetTexture("_MainTex", image.texture);
	}

	public static void ChangeImage(Sprite image)
	{
		GameObject largeSphere = GameObject.Find("ImageSphere");
		Renderer sphereRenderer = largeSphere.GetComponent<Renderer>();
		sphereRenderer.material.SetTexture("_MainTex", image.texture);
	}

	public static void DestroyAllNavigationSpheres()
	{
		if (navigationSpheresParent != null)
		{
			foreach (Transform child in navigationSpheresParent.transform)
			{
				Destroy(child.gameObject);
			}
		}
	}

	public static void NavigateTo(string imageName)
	{
		GameObject largeSphere = GameObject.Find("ImageSphere");
		Renderer sphereRenderer = largeSphere.GetComponent<Renderer>();
		Sprite foundSprite = null;
		foreach (Sprite sprite in images)
		{
			if (sprite.name == imageName)
			{
				foundSprite = sprite;
				break;
			}
		}

		if (foundSprite != null)
		{
			UnityEngine.Debug.Log("Found sprite: " + foundSprite.name);
		}
		else
		{
			UnityEngine.Debug.Log("Sprite not found: " + imageName);
		}
		sphereRenderer.material.SetTexture("_MainTex", foundSprite.texture);
	}

	public static void ChangeInfo(string title, string description)
	{
		ResetQuizAnswers();
		GameObject titleText = GameObject.Find("Title");
		GameObject descriptionText = GameObject.Find("Description");

		titleText.GetComponent<TMPro.TextMeshProUGUI>().text = title;
		descriptionText.GetComponent<TMPro.TextMeshProUGUI>().text = description;
	}

	public static void ShowQuiz(QuizPoint quizPoint)
	{
		ResetQuizAnswers();
		ChangeInfo(quizPoint.title, quizPoint.description);
		GameObject infoPanel = GameObject.Find("InfoPanel");

		foreach (Answer answer in quizPoint.answers)
		{
			GameObject quizAnswer = Instantiate(quizAnswerPrefab, infoPanel.transform);
			quizAnswer.tag = "QuizButton";
			quizAnswer.name = "Answer";
			QuizObjectController quizObjectController = quizAnswer.AddComponent<QuizObjectController>();
			AnswerObjectController answerObjectController = quizAnswer.AddComponent<AnswerObjectController>();

			quizObjectController.objectInfo = quizPoint;
			answerObjectController.objectInfo = answer;

			// Set answer text
			quizAnswer.GetComponentInChildren<TextMeshProUGUI>().text = answer.text;

			// Set button colors
			if (!quizPoint.enabled && quizPoint.selectedAnswer == answer.id)
			{
				ColorBlock colors = quizAnswer.GetComponent<Button>().colors;
				colors.disabledColor = answer.isCorrect ? Color.green : Color.red;
				quizAnswer.GetComponent<Button>().colors = colors;
				quizAnswer.GetComponent<Button>().interactable = false;
			}
		}
	}

	private static void ResetQuizAnswers()
	{
		GameObject infoPanel = GameObject.Find("InfoPanel");
		foreach (Transform child in infoPanel.transform)
		{
			if (child.CompareTag("QuizButton"))
			{
				Destroy(child.gameObject);
			}
		}
	}

    public static void SaveAllPositions<T>(PointsContainer<T> pointData) where T : Point
    {
        DataContainer dataContainer = new DataContainer();

        if (typeof(T) == typeof(NavigationPoint))
        {
            foreach (T point in pointData.points)
            {
                dataContainer.NavigationPoints.Add((NavigationPoint)(object)point);
            }
        }
        else if (typeof(T) == typeof(InfoPoint))
        {
            foreach (T point in pointData.points)
            {
                dataContainer.InfoPoints.Add((InfoPoint)(object)point);
            }
        }
        else if (typeof(T) == typeof(QuizPoint))
        {
            foreach (T point in pointData.points)
            {
                dataContainer.QuizPoints.Add((QuizPoint)(object)point);
            }
        }

        string updatedJson = JsonConvert.SerializeObject(dataContainer, Formatting.Indented);
        File.WriteAllText(jsonFilePath, updatedJson);
    }

    public static bool CheckAnswer(Answer answer)
	{
		if (answer.isCorrect)
		{
			UnityEngine.Debug.Log("Correct answer");
			HapticInteractable.SetHapticImpulse(0.1f, 0.1f);
			return true;
		}
		else
		{
			UnityEngine.Debug.Log("Incorrect answer");
			HapticInteractable.SetHapticImpulse(0.2f, 0.5f);
			return false;
		}
	}

    public static void UpdateQuizPoint(QuizPoint objectInfo)
    {
        string jsonString = File.ReadAllText(jsonFilePath);
        var pointsContainer = JsonConvert.DeserializeObject<PointsContainer<QuizPoint>>(jsonString);

        if (objectInfo is QuizPoint)
        {
            QuizPoint quizPointToUpdate = objectInfo;

            pointsContainer.points.RemoveAll(x => x.x == quizPointToUpdate.x && x.y == quizPointToUpdate.y && x.z == quizPointToUpdate.z);
            pointsContainer.points.Add(quizPointToUpdate);

            SaveAllPositions(pointsContainer);
        }
    }
}