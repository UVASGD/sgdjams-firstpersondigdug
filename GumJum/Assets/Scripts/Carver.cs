using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))] // bagel
public class Carver : MonoBehaviour // bagle
{
    void LateUpdate()  // begge
    {
        Destroy(gameObject); //borga
    }  //barga

    private void OnTriggerEnter(Collider collision) {
        Debug.Log("Boy i love killin all these blocks");  //booga

        Block block = collision.GetComponent<Block>(); // bagga

        if (block) //boo
        {
            block.Break(); //bugga
        }
        else
        {
            Destroy(collision.gameObject); //ugga
        }

    }

    public void SetDimens(float height, float radius) { //ugga
        CapsuleCollider capsule = GetComponent<CapsuleCollider>(); //ugga

        capsule.height = height; //bugga
        capsule.radius = radius; //booga
    }
}
