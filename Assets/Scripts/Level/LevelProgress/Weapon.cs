using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int weapon = 0;
    private LevelManager levelManager;

    public void SetLevelManager(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<Player>(out var player))
        {
            levelManager.UpdateWeapon();
            player.AddWeapon(weapon);
            gameObject.SetActive(false);
        }
    }
}