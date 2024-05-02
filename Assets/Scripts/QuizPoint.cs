using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

[System.Serializable]
public class QuizPoint : Point
{
    public bool enabled;
    public bool isCorrect;
    public int selectedAnswer;
    public List<Answer> answers;
}

[System.Serializable]
public class Answer
{
    public int id;
    public string text;
    public bool isCorrect;
}


[System.Serializable]
public class AnswerObjectController : MonoBehaviour
{
    public Answer objectInfo;
}