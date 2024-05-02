using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    ScreenFader screenFader;

    void Start()
    {
        screenFader = GameObject.Find("ScreenFaderImage").GetComponent<ScreenFader>();
    }

    public void OnUIButtonClick()
    {
        Debug.Log("Button Clicked");
    }

    public void OnNavigationPointClick()
    {
        screenFader.FadeIn(() =>
        {
            NavigationPoint position = transform.parent.GetComponent<NavigationObjectController>().objectInfo;
            PointController.NavigationActivate(position);
            PointController.ChangeInfo(position.title, position.description);
            screenFader.FadeOut();
        });
    }

    public void OnInfoPointClick()
    {
        InfoPoint infoPoint = transform.parent.GetComponent<InfoObjectController>().objectInfo;
        PointController.ChangeInfo(infoPoint.title, infoPoint.description);
    }
    public void OnQuizPointClick()
    {
        QuizPoint quizPoint = transform.parent.GetComponent<QuizObjectController>().objectInfo;
        PointController.ShowQuiz(quizPoint);
    }
    public void OnQuizAnswerClick()
    {
        Answer answer = transform.parent.GetComponent<AnswerObjectController>().objectInfo;
        ColorBlock colors = transform.parent.GetComponent<Button>().colors;
        transform.parent.GetComponent<Button>().colors = colors;
        bool result = PointController.CheckAnswer(answer);

        if (result)
        {
            colors.disabledColor = Color.green;
            transform.parent.GetComponent<Button>().colors = colors;
        }
        else
        {
            colors.disabledColor = Color.red;
            transform.parent.GetComponent<Button>().colors = colors;
        }

        transform.parent.GetComponent<QuizObjectController>().objectInfo.enabled = false;
        transform.parent.GetComponent<QuizObjectController>().objectInfo.isCorrect = result;
        transform.parent.GetComponent<QuizObjectController>().objectInfo.selectedAnswer = answer.id;

        PointController.UpdateQuizPoint(transform.parent.GetComponent<QuizObjectController>().objectInfo);
        PointController.DestroyAllNavigationSpheres();
        PointController.LoadPositionsAndInstantiateSpheres();
        PointController.ShowQuiz(transform.parent.GetComponent<QuizObjectController>().objectInfo);
    }
}
