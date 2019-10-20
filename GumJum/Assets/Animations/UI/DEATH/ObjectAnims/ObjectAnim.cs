using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnim : MonoBehaviour
{
    public GameObject sound;
    protected Animator anim;

    // Start is called before the first frame update
    protected void Start()
    {
        //print("HOLA");
        anim = GetComponent<Animator>();
        //print(anim);
    }

    public void Play(bool level)
    {
        gameObject.SetActive(true);
		Transform[] children = transform.GetComponentsInChildren<Transform>(true);
		foreach (var child in children) {
			child.gameObject.SetActive(true);
		}
		foreach (Transform child in transform) {
			gameObject.SetActive(true);
		}
        anim.SetTrigger("Start");
		Sound();
        StartCoroutine(End(level));
    }

    public void Sound()
    {
        if (sound)
            Instantiate(sound, transform);
    }

    IEnumerator End(bool level)
    {
        yield return new WaitForSeconds(2.5f);
        if (gameObject.transform.childCount > 0)
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        if (level)
            GameManager.Instance.NextLevel();
        else 
            GameManager.Instance.MainMenu();
    }
}
