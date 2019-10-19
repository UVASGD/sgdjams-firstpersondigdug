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

    public static GameManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        lives = 3;
        score = 0;
        level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {

    }
    
    public void Die(DeathType deathType)
    {

    }
}
