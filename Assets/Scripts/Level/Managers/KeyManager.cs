using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public Collider2D[] keysD;
    public Collider2D[] keysE;
    public Collider2D[] keysB;

    private void Awake()
    {
        keysD = new Collider2D[2];
        keysE = new Collider2D[2];
        keysB = new Collider2D[2];

        keysD[0] = transform.Find($"XJoint/D/Keys/blue key").GetComponent<Collider2D>();
        keysD[1] = transform.Find($"XJoint/D/Keys/white key").GetComponent<Collider2D>();
        
        keysE[0] = transform.Find($"XJoint/E/Keys/purple key").GetComponent<Collider2D>();
        keysE[1] = transform.Find($"XJoint/E/Keys/red key").GetComponent<Collider2D>();
        
        keysB[0] = transform.Find($"XJoint/B/Keys/green key").GetComponent<Collider2D>();
        keysB[1] = transform.Find($"XJoint/B/Keys/yellow key").GetComponent<Collider2D>();
    }
}
