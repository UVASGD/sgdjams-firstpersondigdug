using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stickable))]
public class Block : MonoBehaviour
{
    public Event onBreak;
    // Can put stuff here I guess

    public void Break()
    {
        //Debug.Log("oof ouch I die");
        onBreak?.Invoke();
        // Do other stuff
        Destroy(gameObject);
    }
}
