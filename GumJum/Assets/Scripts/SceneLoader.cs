using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void Level()
    {
        GameManager.Instance.monst = 0;
        SceneManager.LoadScene("Level");
    }

    public static void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
