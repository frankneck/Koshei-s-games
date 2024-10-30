using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    [SerializeField] private int score = 0;

    public void AddScore(int additionalScore)
    {
        score += additionalScore;
    }
}
