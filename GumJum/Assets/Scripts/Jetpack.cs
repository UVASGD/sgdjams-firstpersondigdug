using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [SerializeField]
    float thrust = 10f;

    [SerializeField]
    float flyTime = 4f;
    
    [SerializeField]
    float rechargeTime = 2f;

    [SerializeField]
    bool recharges = true;

    [SerializeField]
    float lockTime = 1f;

    //[SerializeField]
    float lockTimer = 0f;

    //[SerializeField]
    bool locked = false;

    //[SerializeField]
    float fuelLevel = 0f;

    [SerializeField]
    JetpackVis vis;

    Rigidbody rb;

    Player player;

    public GameObject jetpack_fx;
    GameObject current_fx;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fuelLevel = 1f;
        player = GetComponent<Player>();
    }

    void Update()
    {

        if (player.can_input && !locked && Input.GetButton("Jump") && fuelLevel > 0f) {
            if (!current_fx)
                current_fx = Instantiate(jetpack_fx, transform);
            rb.AddForce(transform.up * thrust);

            fuelLevel -= Time.deltaTime / flyTime;

            if (fuelLevel < 0f)
            {
                locked = true;
                lockTimer = 0f;
            }
        } 
        else if (recharges && fuelLevel < 1f)
        {
            fuelLevel += Time.deltaTime / rechargeTime;

            if (fuelLevel > 1f)
                fuelLevel = 1f;
        }

        if (recharges && locked)
        {
            lockTimer += Time.deltaTime;

            if (lockTimer > lockTime)
            {
                lockTimer = 0f;
                locked = false;
            }
        }

        if (vis)
            vis.SetFuelLevel(fuelLevel);
    }
}
