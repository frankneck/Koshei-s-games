using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text WeaponText;

    private List<Coins> coinsList;
    private List<Weapon> weaponList;
    private LevelProgress progress;

    private void Awake()
    {
        progress = new LevelProgress();
        coinsList = FindObjectsOfType<Coins>().ToList();

        foreach (Coins coin in coinsList)
        {
            coin.SetLevelManager(this);
        }

        weaponList = FindObjectsOfType<Weapon>().ToList();

        foreach (Weapon weapon in weaponList)
        {
            weapon.SetLevelManager(this);
        }
    }

    public void UpdateScore(int score)
    {
        progress.LevelScore += score;
        ScoreText.text = $"Score: {progress.LevelScore}";
    }

    public void UpdateWeapon()
    {
        progress.LevelWeapon += 1;
        WeaponText.text = $"Weapon: {progress.LevelWeapon}";
    }
}
