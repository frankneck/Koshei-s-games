using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapFinder : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private GameObject cube;

    private Tilemap[] sides;
    private Tilemap currentTilemap;
    private Tilemap lastTilemap;

    private bool shouldRotate;

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
        currentTilemap = sides[0];
        lastTilemap = currentTilemap;
    }
}