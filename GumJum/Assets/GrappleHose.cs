using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHose : MonoBehaviour
{
    public bool hooked;  // True if we hit something

    public float speed;

    GameObject connection;
    Rigidbody rb;
    Collider col;

    int count;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        hooked = false;
        count = 0;
    }

    private void FixedUpdate()
    {
        if(!hooked)
        {
            rb.velocity = transform.forward * speed;
        }
    }

    public void Pump()
    {
        count++;
        print("Pump " + count + "!");
        /*
        if(count >= 10)  // This is for testing!
        {
            Destroy(connection);
            print("Pop!");
        } */
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If we hit something thats not the player
        if(!collision.gameObject.CompareTag("Player"))
        {
            transform.SetParent(collision.transform);
            hooked = true;
            connection = collision.gameObject;
            rb.isKinematic = true;
            col.enabled = false;  // Stop detecting collisions once we hit
        }
    }
}
