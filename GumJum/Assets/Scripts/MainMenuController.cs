using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour {

    public TextMeshProUGUI highScore;

    public Image[] pointers;

    int selected;


    // Start is called before the first frame update
    void Start() {
        Select(0);

        int hs = PlayerPrefs.GetInt("HighScore", 0);

        highScore.text = hs.ToString("D10");
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            Select(selected - 1);
        } else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            Select(selected + 1);
        } else if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
            PerformAction();
        }
    }

    void Select(int i) {
        selected = (i + pointers.Length) % pointers.Length;
        for(int k = 0; k < pointers.Length; k++) {
            pointers[k].gameObject.SetActive(i == k);
        }
    }

    void PerformAction() {
        if(selected == 0) {
            GameManager.Instance.StartGame();
        } else if(selected == 1) {
            SceneLoader.Credits();
        } else if(selected == 2) {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
