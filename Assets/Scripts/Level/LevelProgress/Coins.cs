using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private int score = 5;
    private LevelManager levelManager;

    public void SetLevelManager(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<Player>(out var player))
        {
            levelManager.UpdateScore(score);
            player.AddScore(score);
            gameObject.SetActive(false);
        }
    }
}
