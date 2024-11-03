using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tilemaps;
    [SerializeField] private Transform cameraTransform;

    private Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.up, Vector3.down, Vector3.left, Vector3.right };

    private void SetActiveTilemap()
    {
        float maxDot = -1f;
        int activeIndex = -1;

        for (int i = 0; i < directions.Length; i++)
        {
            float dot = Vector3.Dot(cameraTransform.forward, directions[i]);
            if (dot > maxDot)
            {
                maxDot = dot;
                activeIndex = i;
            }
        }

        Debug.Log(activeIndex);

        for (int i = 0; i < tilemaps.Length; i++)
        {
            tilemaps[i].SetActive(i == activeIndex);
        }
    }
}
