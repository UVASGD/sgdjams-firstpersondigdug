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

    Animator death_anim;

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

        death_anim = GetComponentInChildren<Animator>();
    }
    
    public void Die(DeathType deathType)
    {
        death_anim.SetTrigger(deathType.ToString());
    }

    public void KillMonster(int points)
    {
        int score = PlayerPrefs.GetInt("HighScore", 0);
        PlayerPrefs.SetInt("HighScore", points);
        //update UI
        monst--;
        if (monst <= 0)
            SceneLoader.Level();
    }

    public static void GetPineapple()
    {
        Debug.Log("GOT PINEAPPLE AAAAAAAAAAAAAAAAAAA");
        SceneLoader.Level();
    }
}
