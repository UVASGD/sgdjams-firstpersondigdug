using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RANDOBOI : MonoBehaviour
{
	Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
		startPosition = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
		transform.position = startPosition + Random.insideUnitSphere * 0.07f;
    }
}
