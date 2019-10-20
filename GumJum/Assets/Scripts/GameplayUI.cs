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

    Animator anim;

    public bool level_done;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.RegisterGPUI(this);
        StartLevel();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString("D10");
    }

    public void UpdateLevel(int level)
    {
        levelText.text = level.ToString("D4");
    }

    public void EndLevel()
    {
        anim.SetTrigger("EndLevel");
    }

    public void StartLevel()
    {
        anim.SetTrigger("StartLevel");
    }
}
