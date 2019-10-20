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
        print("HOLA");
        anim = GetComponent<Animator>();
        print(anim);
    }

    public void Play(bool level)
    {
        gameObject.SetActive(true);
        anim.SetTrigger("Start");
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
