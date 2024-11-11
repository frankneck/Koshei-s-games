using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    [SerializeField] private int keys = 0;
    private LevelManager levelManager;

    public void SetLevelManager(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<Player>(out var player))
        {
            levelManager.UpdateKeys();
            player.AddKeys(keys);
            gameObject.SetActive(false);
        }
    }
}