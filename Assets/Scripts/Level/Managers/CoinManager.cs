using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public Collider2D[] coinsA;
    public Collider2D[] coinsB;
    public Collider2D[] coinsC;
    public Collider2D[] coinsD;
    public Collider2D[] coinsE;
    public Collider2D[] coinsF;

    private void Awake()
    {
        coinsA = new Collider2D[21];
        coinsB = new Collider2D[27];
        coinsC = new Collider2D[24];
        coinsD = new Collider2D[30];
        coinsE = new Collider2D[23];
        coinsF = new Collider2D[30];

        for (int i = 0; i < coinsA.Length; i++)
        {
            coinsA[i] = transform.Find($"XJoint/A/Coins/kalina ({i})").GetComponent<Collider2D>();
        }

        for (int i = 0; i < coinsB.Length; i++)
        {
            coinsB[i] = transform.Find($"XJoint/B/Coins/kalina ({i})").GetComponent<Collider2D>();
        }

        for (int i = 0; i < coinsC.Length; i++)
        {
            coinsC[i] = transform.Find($"XJoint/C/Coins/kalina ({i})").GetComponent<Collider2D>();
        }

        for (int i = 0; i < coinsD.Length; i++)
        {
            coinsD[i] = transform.Find($"XJoint/D/Coins/kalina ({i})").GetComponent<Collider2D>();
        }

        for (int i = 0; i < coinsE.Length; i++)
        {
            coinsE[i] = transform.Find($"XJoint/E/Coins/kalina ({i})").GetComponent<Collider2D>();
        }

        for (int i = 0; i < coinsF.Length; i++)
        {
            coinsF[i] = transform.Find($"XJoint/F/Coins/kalina ({i})").GetComponent<Collider2D>();
        }
    }
}
