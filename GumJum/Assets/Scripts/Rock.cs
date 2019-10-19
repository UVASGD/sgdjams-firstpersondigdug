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
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        col = GetComponent<Collider>();
        col.isTrigger = false;

        Debug.Log("ugga bugga");
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
        Debug.Log("COLLIDING AA");
        Debug.Log("FALLING: " + falling.ToString());

        if (!falling) return;

        Debug.Log("faaaaaalling");

        Block block = collision.gameObject.GetComponent<Block>();

        if (block && block.transform.position.y < transform.position.y && (transform.position.x - block.transform.position.x) < 0.001f)
        {
            Debug.Log("AAAAA GUCK I HIT SOMETHING");
            falling = false;
            block.onBreak += SupportBreak;
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
