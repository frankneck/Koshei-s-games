using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TMP_Text ScoreText;

    private List<Pickuper> coinsList;
    private LevelProgress progress;

    private void Awake()
    {
        progress = new LevelProgress();
        coinsList = FindObjectsOfType<Pickuper>().ToList();

        foreach (Pickuper coin in coinsList)
        {
            coin.SetLevelManager(this);
        }
    }

    public void UpdateScore(int score)
    {
        progress.LevelScore += score;
        ScoreText.text = $"Score: { progress.LevelScore }";
    }
}
