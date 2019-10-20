using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class Rock : MonoBehaviour
{
    Rigidbody rb;
    Collider col;
    Block supportBlock;

    [SerializeField]
    float fallSpeed;

    bool falling = false;

    private void Awake()
    {
        //falling = true;
        rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;

        col = GetComponent<Collider>();
        //col.isTrigger = false;
    }

    private void Update()
    {
        if (falling)
        {
            transform.position -= new Vector3(0f, fallSpeed, 0f) * Time.deltaTime;
        }
    }

    public void SetSupportBlock(Block _block)
    {
        supportBlock = _block;
        supportBlock.onBreak += SupportBreak;
    }

    void SupportBreak()
    {
        falling = true;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("FALLING at collide start: " + falling.ToString());

        if (!falling)
        {
            Debug.Log("Breaking");
            return;
        }

        Debug.Log("faaaaaalling");

        RockStop block = collision.gameObject.GetComponent<RockStop>();

        if (block && block.transform.position.y < transform.position.y && (transform.position.x - block.transform.position.x) < 0.001f)
        {
            Debug.Log("AAAAA GUCK I HIT SOMETHING");
            falling = false;
            block.onDestroy += SupportBreak;
            return;
        }

        Squishable squish = collision.gameObject.GetComponent<Squishable>();

        if (squish)
        {
            squish.Squish();
            return;
        }
    }
}
