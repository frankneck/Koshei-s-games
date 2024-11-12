using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public Collider2D[] doorsD;
    public Collider2D[] doorsE;
    public Collider2D[] doorsB;

    private void Awake()
    {
        doorsD = new Collider2D[4];
        doorsE = new Collider2D[4];
        doorsB = new Collider2D[4];

        doorsD[0] = transform.Find($"XJoint/D/Doors/green door").GetComponent<Collider2D>();
        doorsD[1] = transform.Find($"XJoint/D/Doors/green door/green door 1").GetComponent<Collider2D>();
        doorsD[2] = transform.Find($"XJoint/D/Doors/purple door").GetComponent<Collider2D>();
        doorsD[3] = transform.Find($"XJoint/D/Doors/purple door/purple door 1").GetComponent<Collider2D>();

        doorsE[0] = transform.Find($"XJoint/E/Doors/white door").GetComponent<Collider2D>();
        doorsE[1] = transform.Find($"XJoint/E/Doors/white door/white door 1").GetComponent<Collider2D>();
        doorsE[2] = transform.Find($"XJoint/E/Doors/yellow door").GetComponent<Collider2D>();
        doorsE[3] = transform.Find($"XJoint/E/Doors/yellow door/yellow door 1").GetComponent<Collider2D>();

        doorsB[0] = transform.Find($"XJoint/B/Doors/blue door").GetComponent<Collider2D>();
        doorsB[1] = transform.Find($"XJoint/B/Doors/blue door/blue door 1").GetComponent<Collider2D>();
        doorsB[2] = transform.Find($"XJoint/B/Doors/red door").GetComponent<Collider2D>();
        doorsB[3] = transform.Find($"XJoint/B/Doors/red door/red door 1").GetComponent<Collider2D>();
    }
}
