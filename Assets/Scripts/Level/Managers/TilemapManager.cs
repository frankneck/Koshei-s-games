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

        sides[0] = transform.Find("Yjoint/A/FrontA").GetComponent<Tilemap>();  //A
        sides[1] = transform.Find("Yjoint/B/FrontB").GetComponent<Tilemap>();  //B
        sides[2] = transform.Find("Yjoint/C/FrontC").GetComponent<Tilemap>();  //C
        sides[3] = transform.Find("Yjoint/D/FrontD").GetComponent<Tilemap>();  //D
        sides[4] = transform.Find("Yjoint/E/FrontE").GetComponent<Tilemap>();  //E
        sides[5] = transform.Find("Yjoint/F/FrontF").GetComponent<Tilemap>();  //F
    }

    private void Start()
    {
        currentTilemap = sides[0];
        lastTilemap = currentTilemap;
    }
}