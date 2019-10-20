using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockStop : MonoBehaviour
{
    public Event onDestroy;

    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }
}
