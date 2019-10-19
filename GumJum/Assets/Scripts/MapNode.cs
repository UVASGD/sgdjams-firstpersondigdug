using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Event();

public class MapNode : MonoBehaviour
{
    //public Event onBreak;

    public bool Broken { get; private set; } = false;

    public Block Block { get; private set; }

    private void Awake()
    {
        Block = GetComponentInChildren<Block>();

        Block.onBreak += BlockBroke;

    }

    void BlockBroke()
    {
        Debug.Log("ops I brok");
        //onBreak?.Invoke();
        Broken = true;
    }

    public void Break()
    {
        if (!Broken && Block)
        {
            Block.Break();
        }

        Broken = true;
    }
}
