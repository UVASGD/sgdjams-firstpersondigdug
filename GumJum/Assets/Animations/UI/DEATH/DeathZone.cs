using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	[SerializeField]
    Transform monster_kill;
	[SerializeField]
	Transform pineapple;
	[SerializeField]
	Transform crush;
	[SerializeField]
	Transform goggle;
	[SerializeField]
	Transform fall;
    //Transform dragon;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        monster_kill = transform.FindDeepChild("MonsterKillAnim");
        pineapple = transform.FindDeepChild("PineappleAnim");
        crush = transform.FindDeepChild("CrushAnim");
        goggle = transform.FindDeepChild("GoggleAnim");
        fall = transform.FindDeepChild("FallAnim");

		monster_kill.gameObject.SetActive(false);
		pineapple.gameObject.SetActive(false);
		crush.gameObject.SetActive(false);
		goggle.gameObject.SetActive(false);
		fall.gameObject.SetActive(false);
		//dragon = transform.FindDeepChild("DragonAnim");
	}

    public void NextLevel()
    {
        GameManager.Instance.NextLevel();
    }

    public void MainMenu()
    {
        GameManager.Instance.MainMenu();
    }

    public void EndLevel(string type)
    {
        anim.SetTrigger("EndFade");
        switch (type)
        {
            case "MonsterKill":
				print("Monster Kill!");
                monster_kill.gameObject.SetActive(true);
                monster_kill.GetComponent<ObjectAnim>().Play(true);
                break;
            case "Pineapple":
				print("Pineapple!");
				pineapple.gameObject.SetActive(true);
                pineapple.GetComponent<ObjectAnim>().Play(true);
                break;
            case "Crush":
				print("Crush!");
				crush.gameObject.SetActive(true);
                crush.GetComponent<ObjectAnim>().Play(false);
                break;
            case "Goggle":
				print("Goggle!");
				goggle.gameObject.SetActive(true);
                goggle.GetComponent<ObjectAnim>().Play(false);
                break;
            case "Fall":
				print("Fall!");
				fall.gameObject.SetActive(true);
                fall.GetComponent<ObjectAnim>().Play(false);
                break;
                /*
            case "Dragon":
            dragon.Play();
            break;*/
        }
    } 
}
