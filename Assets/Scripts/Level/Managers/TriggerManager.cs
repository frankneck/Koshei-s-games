using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TriggerManager : MonoBehaviour
{
    public Collider2D[,] triggers;
    public Collider2D[] currentTriggers;
    public Collider2D[] lastTriggers;

    private void Awake()
    {
        triggers = new Collider2D[6, 8];
        currentTriggers = new Collider2D[8];
        lastTriggers = new Collider2D[8];

        triggers[0, 0] = transform.Find("A1").GetComponent<Collider2D>();  
        triggers[0, 1] = transform.Find("A2").GetComponent<Collider2D>();  
        triggers[0, 2] = transform.Find("A3").GetComponent<Collider2D>();  
        triggers[0, 3] = transform.Find("A4").GetComponent<Collider2D>();  
        triggers[0, 4] = transform.Find("A5").GetComponent<Collider2D>();  
        triggers[0, 5] = transform.Find("A6").GetComponent<Collider2D>();  
        triggers[0, 6] = transform.Find("A7").GetComponent<Collider2D>();  
        triggers[0, 7] = transform.Find("A8").GetComponent<Collider2D>();

        triggers[1, 0] = transform.Find("B1").GetComponent<Collider2D>();
        triggers[1, 1] = transform.Find("B2").GetComponent<Collider2D>();
        triggers[1, 2] = transform.Find("B3").GetComponent<Collider2D>();
        triggers[1, 3] = transform.Find("B4").GetComponent<Collider2D>();
        triggers[1, 4] = transform.Find("B5").GetComponent<Collider2D>();
        triggers[1, 5] = transform.Find("B6").GetComponent<Collider2D>();
        triggers[1, 6] = transform.Find("B7").GetComponent<Collider2D>();
        triggers[1, 7] = transform.Find("B8").GetComponent<Collider2D>();

        triggers[2, 0] = transform.Find("C1").GetComponent<Collider2D>();
        triggers[2, 1] = transform.Find("C2").GetComponent<Collider2D>();
        triggers[2, 2] = transform.Find("C3").GetComponent<Collider2D>();
        triggers[2, 3] = transform.Find("C4").GetComponent<Collider2D>();
        triggers[2, 4] = transform.Find("C5").GetComponent<Collider2D>();
        triggers[2, 5] = transform.Find("C6").GetComponent<Collider2D>();
        triggers[2, 6] = transform.Find("C7").GetComponent<Collider2D>();
        triggers[2, 7] = transform.Find("C8").GetComponent<Collider2D>();

        triggers[3, 0] = transform.Find("D1").GetComponent<Collider2D>();
        triggers[3, 1] = transform.Find("D2").GetComponent<Collider2D>();
        triggers[3, 2] = transform.Find("D3").GetComponent<Collider2D>();
        triggers[3, 3] = transform.Find("D4").GetComponent<Collider2D>();
        triggers[3, 4] = transform.Find("D5").GetComponent<Collider2D>();
        triggers[3, 5] = transform.Find("D6").GetComponent<Collider2D>();
        triggers[3, 6] = transform.Find("D7").GetComponent<Collider2D>();
        triggers[3, 7] = transform.Find("D8").GetComponent<Collider2D>();

        triggers[4, 0] = transform.Find("E1").GetComponent<Collider2D>();
        triggers[4, 1] = transform.Find("E2").GetComponent<Collider2D>();
        triggers[4, 2] = transform.Find("E3").GetComponent<Collider2D>();
        triggers[4, 3] = transform.Find("E4").GetComponent<Collider2D>();
        triggers[4, 4] = transform.Find("E5").GetComponent<Collider2D>();
        triggers[4, 5] = transform.Find("E6").GetComponent<Collider2D>();
        triggers[4, 6] = transform.Find("E7").GetComponent<Collider2D>();
        triggers[4, 7] = transform.Find("E8").GetComponent<Collider2D>();

        triggers[5, 0] = transform.Find("F1").GetComponent<Collider2D>();  
        triggers[5, 1] = transform.Find("F2").GetComponent<Collider2D>();  
        triggers[5, 2] = transform.Find("F3").GetComponent<Collider2D>();  
        triggers[5, 3] = transform.Find("F4").GetComponent<Collider2D>();  
        triggers[5, 4] = transform.Find("F5").GetComponent<Collider2D>();  
        triggers[5, 5] = transform.Find("F6").GetComponent<Collider2D>();  
        triggers[5, 6] = transform.Find("F7").GetComponent<Collider2D>();  
        triggers[5, 7] = transform.Find("F8").GetComponent<Collider2D>();  
    }

    private void Start()
    {
        for (int i = 0; i < currentTriggers.Length; i++)
        {
            currentTriggers[i] = triggers[0, i];
        }
        for (int i = 0; i < lastTriggers.Length; i++)
        {
            lastTriggers[i] = currentTriggers[i];
        }
    }
}
