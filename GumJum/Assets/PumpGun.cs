using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpGun : MonoBehaviour
{
    public GameObject projectile;
    public Transform spawnPoint;
    public Transform renderPoint;

    public float maxRange;

    bool fired = false;  // True if "hose" as been fired
    GameObject hooked;  // GameObject that hose has hooked onto. Typically an enemy if you have good aim. (Change GameObject to enemy script)

    GameObject hose;  // Reference to actual projectile spawned
    GrappleHose hoseGrapple;  // Reference to GrappleHose script

    LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(fired)
        {
            Vector3[] linePos = { renderPoint.position, hose.transform.position };
            line.SetPositions(linePos);
            if(hoseGrapple.hooked)  // If we hit something with the hose/grapple/pump thingy
            {
                if(Input.GetButtonDown("Fire2"))
                {
                    hoseGrapple.Pump();
                }
                
            }
            if (Input.GetButtonDown("Fire3"))
            {
                Recall();
            }
        }
        else
        {
            if(Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
            
        }
    }

    public void Shoot()
    {
        if (!fired)
        {
            hose = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation) as GameObject;
            hoseGrapple = hose.GetComponent<GrappleHose>();
            fired = true;
            line.enabled = true;
        }
    }

    public void Recall()
    {
        if(fired)
        {
            Destroy(hose);
            fired = false;
            hoseGrapple = null;
            line.enabled = false;
        }
    }
}
