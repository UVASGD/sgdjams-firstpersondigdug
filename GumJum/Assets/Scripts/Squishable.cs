using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squishable : MonoBehaviour
{
    Player player;
    Monster monster;

    private void Awake()
    {
        if (GetComponent<Player>())
            player = GetComponent<Player>();
        else if (GetComponent<Monster>())
            monster = GetComponent<Monster>();
    }

    public void Squish()
    {
        if (player)
        {
            player.Die(DeathType.Crush);
        }
        else if (monster)
        {
            monster.Squish();
        }
    }
}
