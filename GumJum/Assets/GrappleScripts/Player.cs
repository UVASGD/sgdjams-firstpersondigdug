﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public bool can_input = true;

    // Start is called before the first frame update
    void Start()
    {
        can_input = true;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
