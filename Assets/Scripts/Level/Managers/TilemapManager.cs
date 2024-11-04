using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapFinder : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private GameObject cube;

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
    private Tilemap lastTilemap;
    private ContactFilter2D triggerFilter;
    private Collider2D[] collides = new Collider2D[16]; // Буфер для пересечений
    private int count; // Счетчик для пересечений

    private bool shouldRotate;

    private volatile bool downTrigger1;
    private volatile bool downTrigger2;

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
        lastTilemap = currentTilemap;

        // Отключение всех тайлмапов кроме A
        for (int i = 2; i < sides.Length; i++)
        {
            sides[i].GetComponent<Collider2D>().enabled = false;
        }


    }

    private void Update()
    {
        shouldRotate = downTriggerOne.GetComponent<DownTrigger>().shouldRotate;

        downTrigger1 = downTriggerOne.GetComponent<DownTrigger>().isDownTrigger;
        downTrigger2 = downTriggerTwo.GetComponent<DownTrigger>().isDownTrigger;

        DefineCurrentSide();
    }

    private void DefineCurrentSide()
    {
        Debug.Log($"Текущая тайлмап: {currentTilemap.name}");
        Debug.Log($"downTrigger1: {downTrigger1}, downTrigger2: {downTrigger2}");

        switch (System.Array.IndexOf(sides, currentTilemap))
        {
            case 0: // For A

                if ((downTrigger1 || downTrigger2) && !shouldRotate)
                {
                    Debug.Log("Нижний триггер A");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[5]; // F
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;

                    // Обнуление isDownTrigger
                    downTrigger1 = false;
                    downTrigger2 = false;
                    
                    break;
                }

                //    if ((downTrigger1 || downTrigger2) && !shouldRotate)
                //    {
                //        currentTilemap = sides[4]; // E
                //        break;
                //    }

                //    count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[2]; // C
                //        break;
                //    }

                //    count = aboveCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[1]; // B
                //        break;
                //    }
                break;

                //case 1:     // For B
                //    count = groundCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[0]; // A
                //        break;
                //    }

                //    count = leftWallCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[4]; // E
                //        break;
                //    }

                //    count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    { 
                //        currentTilemap = sides[2]; // C
                //        break;
                //    }

                //    count = aboveCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[5]; // F
                //        break;
                //    }
                break;
                //case 2:     // For C
                //    count = groundCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[5]; // F
                //        break;
                //    }

                //    count = leftWallCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[0]; // A
                //        break;
                //    }

                //    count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[3]; // D
                //        break;
                //    }

                //    count = aboveCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[1]; // B
                //        break;
                //    }
                break;
                //case 3:     // For D
                //    count = groundCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[5]; // F
                //        break;
                //    }

                //    count = leftWallCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[2]; // C
                //        break;
                //    }

                //    count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[4]; // E
                //        break;
                //    }

                //    count = aboveCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[1]; // B
                //        break;
                //    }
                break;
                //case 4:     // For E
                //    count = groundCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[5]; // F
                //        break;
                //    }

                //    count = leftWallCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[3]; // D
                //        break;
                //    }

                //    count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[0]; // A
                //        break;
                //    }

                //    count = aboveCollider.OverlapCollider(triggerFilter, collides);
                //    if (count > 0)
                //    {
                //        currentTilemap = sides[1]; // B
                //        break;
                //}
                break;
            case 5:     // For F
                if ((downTrigger1 || downTrigger2) && !shouldRotate)
                {
                    Debug.Log("Нижний триггер F");
                    lastTilemap = currentTilemap;
                    currentTilemap = sides[3]; // D
                    currentTilemap.transform.GetComponent<Collider2D>().enabled = true;
                    lastTilemap.transform.GetComponent<Collider2D>().enabled = false;

                    // Обнуление isDownTrigger
                    downTrigger1 = false;
                    downTrigger2 = false;
                    break;
                }
                else
                    Debug.Log("Down Trigger false");


                //count = leftWallCollider.OverlapCollider(triggerFilter, collides);
                //if (count > 0)
                //{
                //    currentTilemap = sides[4]; // E
                //    break;
                //}

                //count = rightWallCollider.OverlapCollider(triggerFilter, collides);
                //if (count > 0)
                //{
                //    currentTilemap = sides[2]; // C
                //    break;
                //}

                //count = aboveCollider.OverlapCollider(triggerFilter, collides);
                //if (count > 0)
                //{
                //    currentTilemap = sides[0]; // A
                //    break;
                //}
                break;
            
            default:
                break;
        }
    }
}