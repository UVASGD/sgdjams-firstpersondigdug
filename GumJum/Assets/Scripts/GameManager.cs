using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public int lives;
    [HideInInspector]
    public int score;
    [HideInInspector]
    public int level;
    [HideInInspector]
    public int dicks;
    [HideInInspector]
    public int monst;

    public static GameManager Instance;

    Animator end_anim;
    [HideInInspector]
    public Player player { get { return FindObjectOfType<Player>(); } }

    GameplayUI gpUI;

    // Start is called before the first frame update
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        lives = 3;
        score = 0;
        level = 1;
        monst = 0;

        end_anim = GetComponentInChildren<Animator>();
    }
    
    public void Die(DeathType deathType)
    {
        end_anim.SetTrigger(deathType.ToString());
    }

    public void NextLevel()
    {
        level++;
        if (gpUI)
            gpUI.UpdateLevel(level);
    }

    public void KillMonster(int points)
    {
        score += points;
        if (gpUI)
            gpUI.UpdateScore(score);

        //update UI
        monst--;
        if (monst <= 0)
            end_anim.SetTrigger("MonsterKill");
    }

    public void SaveScore()
    {
        int highscore = PlayerPrefs.GetInt("HighScore", 0);

        if (score > highscore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    public void GetPineapple()
    {
        Debug.Log("GOT PINEAPPLE AAAAAAAAAAAAAAAAAAA");
        score += 500;

        if (gpUI)
            gpUI.UpdateScore(score);

        end_anim.SetTrigger("Pineapple");
    }

    public void RegisterGPUI(GameplayUI _gpUI)
    {
        gpUI = _gpUI;
    }
}
