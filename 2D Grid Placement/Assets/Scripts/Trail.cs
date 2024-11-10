using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    private Vector3Int currentPosition;
    private Vector3Int answerPosition;
    private float rotation;
    private float answerRotation;
    private bool isCorrect = false;

    public Vector3Int CurrentPos { get => currentPosition; set => currentPosition = value; }
    public Vector3Int AnswerPos { get => answerPosition; set => answerPosition = value; }
    public float Rotation { get => rotation; set => rotation = value; }
    public float AnswerRotation { get => answerRotation; set => answerRotation = value; }
    public bool IsCorrect { get => isCorrect; set => isCorrect = value; }

    public void CheckAnswer()
    {
        if (currentPosition == answerPosition && rotation == answerRotation)
        {
            isCorrect = true;
        }
        else
        {
            isCorrect = false;
        }
    }
}
