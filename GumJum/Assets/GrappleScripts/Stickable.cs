using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void GameEvent();

public class Stickable : MonoBehaviour
{
    public GameEvent OnBreak;
    [HideInInspector]
    public bool is_monster;

    Monster monster;

    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<Monster>())
        {
            is_monster = true;
            monster = GetComponent<Monster>();
        }
    }


    public void Release()
    {
    }

    public void Pump()
    {
        if (monster) {
            monster.Inflate();
        }
    }

    private void OnDestroy()
    {
        OnBreak?.Invoke();
    }
}
