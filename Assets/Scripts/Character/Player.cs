using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private int weapon = 0;
    [SerializeField] private int keys = 0;

    public void AddScore(int additionalScore)
    {
        score += additionalScore;
    }

    public void AddWeapon(int additionalWeapon)
    {
        weapon += additionalWeapon;
    }

    public void AddKeys(int additionalWeapon)
    {
        keys += additionalWeapon;
    }
}
