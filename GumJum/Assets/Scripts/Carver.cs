using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Carver : MonoBehaviour
{
    void LateUpdate()
    {
        //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision) {
        Debug.Log("Boy i love killin");
        Destroy(collision.gameObject);
    }

    public void SetDimens(float height, float radius) {
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();

        capsule.height = height;
        capsule.radius = radius;
    }
}
