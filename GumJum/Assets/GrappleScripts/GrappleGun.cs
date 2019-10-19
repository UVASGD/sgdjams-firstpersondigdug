using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class GrappleGun : MonoBehaviour
{
    Player player;
    GrappleHook grapple_hook;
    [HideInInspector]
    public Transform start_point;

    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponentInParent<Player>();
        grapple_hook = GetComponentInChildren<GrappleHook>();
        start_point = transform.FindDeepChild("start_point");
    }

    private void Start()
    {
        PickUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (!grapple_hook)
            return;
        if (player.can_input)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                grapple_hook.Fire(transform.forward);
            }
            if (Input.GetButtonDown("Fire2"))
            {
                print(grapple_hook);
                grapple_hook.Crank();
            }
        }
    }

    public void PickUp() 
    {
        if (grapple_hook) 
        {
            grapple_hook.ResetState();
        }
    }
}
