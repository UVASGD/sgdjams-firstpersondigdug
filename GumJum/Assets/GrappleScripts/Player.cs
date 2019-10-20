using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeathType { Crush, Goggle, Alien, Dragon, Fall }

[RequireComponent(typeof(Squishable))]
public class Player : MonoBehaviour
{
    [HideInInspector]
    public bool can_input = true;

    // Start is called before the first frame update
    void Start()
    {
        can_input = true;   
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < (GameManager.Instance.min_y - 50))
        {
            print("FALL: " + GameManager.Instance.min_y);
            Die(DeathType.Fall);
        }
    }

    public void Die(DeathType deathType)
    {
        if (!can_input)
            return;
        can_input = false;
        GameManager.Instance.Die(deathType);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Goggle"))
        {
            Die(DeathType.Goggle);
        }
        /*
        else if (collision.collider.CompareTag("Dragon"))
        {
            Die(DeathType.Dragon);
        }*/
    }
}
