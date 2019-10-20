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

    public void KillMonster(int points)
    {
        int score = PlayerPrefs.GetInt("HighScore", 0);
        PlayerPrefs.SetInt("HighScore", score+points);
        //update UI
        monst--;
        if (monst <= 0)
            end_anim.SetTrigger("MonsterKill");
    }

    public void GetPineapple()
    {
        Debug.Log("GOT PINEAPPLE AAAAAAAAAAAAAAAAAAA");
        int score = PlayerPrefs.GetInt("HighScore", 0);
        PlayerPrefs.SetInt("HighScore", 500+score);
        end_anim.SetTrigger("Pineapple");
    }
}
