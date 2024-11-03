using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapFinder : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private GameObject cube;

    [Header("Character colliders")]
    [SerializeField] private CapsuleCollider2D groundCollider;
    [SerializeField] private CapsuleCollider2D aboveCollider;
    [SerializeField] private CapsuleCollider2D leftWallCollider;
    [SerializeField] private CapsuleCollider2D rightWallCollider;

    [Header("Layer for triggers")]
    [SerializeField] private LayerMask triggerLayer;

    [Header("Ttriggers")]
    [SerializeField] private GameObject downTriggerOne;
    [SerializeField] private GameObject downTriggerTwo;
    //[SerializeField] private GameObject upTrigger;
    //[SerializeField] private GameObject leftTrigger;
    //[SerializeField] private GameObject rightTrigger;

    private Tilemap[] sides;
    private Tilemap currentTilemap;
    private ContactFilter2D triggerFilter;
    private Collider2D[] collides = new Collider2D[16]; // Буфер для пересечений
    private int count; // Счетчик для пересечений

    private void Awake()
    {
        sides = new Tilemap[6];

        sides[0] = cube.transform.Find("A/FrontA").GetComponent<Tilemap>();  //A
        sides[1] = cube.transform.Find("B/FrontB").GetComponent<Tilemap>();  //B
        sides[2] = cube.transform.Find("C/FrontC").GetComponent<Tilemap>();  //C
        sides[3] = cube.transform.Find("D/FrontD").GetComponent<Tilemap>();  //D
        sides[4] = cube.transform.Find("E/FrontE").GetComponent<Tilemap>();  //E
        sides[5] = cube.transform.Find("F/FrontF").GetComponent<Tilemap>();  //F
    }

    private void Start()
    {
        // Настройка для слоя триггеры
        triggerFilter.SetLayerMask(triggerLayer);
        triggerFilter.useLayerMask = true;
        triggerFilter.useTriggers = false;

        // Текущая тайлмап - А
        currentTilemap = sides[0];
        var lastTilemap = currentTilemap;

        // Отключение всех тайлмапов кроме A
        for (int i = 2; i < sides.Length; i++)
        {
            sides[i].GetComponent<Collider2D>().enabled = false;
        }
    }

    private void FixedUpdate()
    {
        DefineCurrentSide();
    }

    private void DefineCurrentSide()
    {
        switch (System.Array.IndexOf(sides, currentTilemap))
        {
            case 0: // For A
                if (downTriggerOne.GetComponent<DownTrigger>().isDownTrigger || downTriggerTwo.GetComponent<DownTrigger>().isDownTrigger)
                {
                    var lastTilemap = currentTilemap;
                    currentTilemap = sides[5]; // F
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;
                    break;
                }
                
                count = leftWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[4]; // E
                    break;
                }

                count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[2]; // C
                    break;
                }

                count = aboveCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[1]; // B
                    break;
                }
                break;

            case 1:     // For B
                count = groundCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[0]; // A
                    break;
                }

                count = leftWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[4]; // E
                    break;
                }

                count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                { 
                    currentTilemap = sides[2]; // C
                    break;
                }

                count = aboveCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[5]; // F
                    break;
                }
                break;
            case 2:     // For C
                count = groundCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[5]; // F
                    break;
                }

                count = leftWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[0]; // A
                    break;
                }

                count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[3]; // D
                    break;
                }

                count = aboveCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[1]; // B
                    break;
                }
                break;
            case 3:     // For D
                count = groundCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[5]; // F
                    break;
                }

                count = leftWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[2]; // C
                    break;
                }

                count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[4]; // E
                    break;
                }

                count = aboveCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[1]; // B
                    break;
                }
                break;
            case 4:     // For E
                count = groundCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[5]; // F
                    break;
                }

                count = leftWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[3]; // D
                    break;
                }

                count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[0]; // A
                    break;
                }

                count = aboveCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[1]; // B
                    break;
                }
                break;
            case 5:     // For F
                count = groundCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[1]; // B
                    break;
                }

                count = leftWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[4]; // E
                    break;
                }

                count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[2]; // C
                    break;
                }

                count = aboveCollider.OverlapCollider(triggerFilter, collides);
                if (count > 0)
                {
                    currentTilemap = sides[0]; // A
                    break;
                }
                break;
            
            default:
                break;
        }
    }
}


/*
Что имеем

Два варианта 

1) У меня есть триггер вниз (В самом тригере)
2) Когда он срабатывает текущий тайлмап равен по условию
 


 */