using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public Tilemap[] sides;
    public Tilemap currentTilemap;
    public Tilemap lastTilemap;

    private void Awake()
    {
        sides = new Tilemap[6];

        sides[0] = transform.Find("A/FrontA").GetComponent<Tilemap>();  //A
        sides[1] = transform.Find("B/FrontB").GetComponent<Tilemap>();  //B
        sides[2] = transform.Find("C/FrontC").GetComponent<Tilemap>();  //C
        sides[3] = transform.Find("D/FrontD").GetComponent<Tilemap>();  //D
        sides[4] = transform.Find("E/FrontE").GetComponent<Tilemap>();  //E
        sides[5] = transform.Find("F/FrontF").GetComponent<Tilemap>();  //F
    }

    private void Start()
    {
        currentTilemap = sides[0];
        lastTilemap = currentTilemap;
    }
}