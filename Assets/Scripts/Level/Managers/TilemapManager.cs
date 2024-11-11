using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public Tilemap[] sides;
    public Tilemap currentTilemap;
    public Tilemap lastTilemap;

    private void Start()
    {
        currentTilemap = sides[0];
        lastTilemap = currentTilemap;
    }
}