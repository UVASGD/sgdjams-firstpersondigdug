using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void GameEvent();

public class Stickable : MonoBehaviour
{
    public GameEvent OnBreak;
    [HideInInspector]
    public bool is_monster;

    // Start is called before the first frame update
    void Awake()
    { 
      //  if (GetComponent<Monster>())
      //      is_monster = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        OnBreak?.Invoke();
    }
}
