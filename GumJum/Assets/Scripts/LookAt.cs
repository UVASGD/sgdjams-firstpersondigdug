using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;

    public bool reverse = false;

    void Update()
    {
        if (target && reverse)
            transform.LookAt(2 * transform.position - target.position);
        else if (target && !reverse)
            transform.LookAt(target.position);
    }
}
