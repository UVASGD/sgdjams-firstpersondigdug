using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI levelText;

    [SerializeField]
    TextMeshProUGUI scoreText;

    private void Start()
    {
        GameManager.Instance.RegisterGPUI(this);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
    }
}
