using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    [HideInInspector]
    public int min_y;

    public static GameManager Instance;

    Animator end_anim;
    [HideInInspector]
    public Player player { get { return FindObjectOfType<Player>(); } }

    GameplayUI gpUI;
    bool level_done;

    Camera end_camera;
    DeathZone deathZone;

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

        end_anim = GetComponentInChildren<Animator>();
        end_camera = transform.FindDeepChild("Camera").GetComponent<Camera>();
        deathZone = GetComponentInChildren<DeathZone>();
        DontDestroyOnLoad(gameObject);
    }
    
    public void Die(DeathType deathType)
    {
        if (level_done)
            return;
        StartCoroutine(EndLevel(deathType.ToString()));
    }

    public void StartGame()
    {
        lives = 3;
        score = 0;
        level = 1;
        monst = 0;
        min_y = -500;
        level_done = false;
        SceneLoader.Level();
    }

    public void NextLevel()
    {
        level++;
        level_done = false;
        score += 400;
        if (gpUI)
        {
            gpUI.UpdateLevel(level);
            gpUI.UpdateScore(level);
        }
        min_y = -500;
        SceneLoader.Level();
    }

    public void MainMenu()
    {
        SaveScore();
        DeactivateCamera();
        SceneLoader.MainMenu();
    }


    public void KillMonster(int points)
    {
        if (level_done)
            return;
        score += points;
        if (gpUI)
            gpUI.UpdateScore(score);

        //update UI
        monst--;
        if (monst <= 0)
        {
            StartCoroutine(EndLevel("MonsterKill"));
        }
    }

    IEnumerator EndLevel(string type)
    {
        level_done = true;
        gpUI.EndLevel();
        while (!gpUI.level_done)
            yield return null;
        ActivateCamera();
        deathZone.EndLevel(type);
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
        if (level_done)
            return;
        Debug.Log("GOT PINEAPPLE AAAAAAAAAAAAAAAAAAA");
        score += 500;

        if (gpUI)
            gpUI.UpdateScore(score);

        StartCoroutine(EndLevel("Pineapple"));
    }

    public void RegisterGPUI(GameplayUI _gpUI)
    {
        DeactivateCamera();
        gpUI = _gpUI;
        gpUI.UpdateScore(score);
        gpUI.UpdateLevel(level);
    }

    void ActivateCamera()
    {
        Camera.main.enabled = false;
        player.transform.parent.gameObject.SetActive(false);
        end_camera.enabled = true;
    }

    void DeactivateCamera()
    {
        if (Camera.main)
        {
            end_camera.enabled = false;
            Camera.main.enabled = true;
        }
    }
}
