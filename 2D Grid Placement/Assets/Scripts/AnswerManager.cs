using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    [SerializeField] private List<Answer> answers;

    //Set the answer position and rotation for each trail
    public void SetAnswerList(List<GameObject> trail)
    {
        for (int i = 0; i < trail.Count; i++)
        {
            trail[i].GetComponent<Trail>().AnswerPos = answers[i].position;
            trail[i].GetComponent<Trail>().AnswerRotation = answers[i].rotation;
        }
    }

    //Check if all the trail is placed correctly
    public bool CheckAnswers(List<GameObject> trail)
    {
        for (int i = 0; i < trail.Count; i++)
        {
            if (!trail[i].GetComponent<Trail>().IsCorrect)
            {
                return false;
            }
        }
        return true;
    }

}

[System.Serializable]
public class Answer
{
    [SerializeField] public Vector3Int position;
    [SerializeField] public int rotation;
}

