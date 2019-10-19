using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JetpackVis : MonoBehaviour
{
    [SerializeField]
    Image fuelMeter;

    Vector2 originalSize;

    private void Awake()
    {
        if (fuelMeter == null)
        {
            Destroy(gameObject);
            return;
        }

        originalSize = fuelMeter.rectTransform.sizeDelta;
    }

    public void SetFuelLevel(float _fuelLevel)
    {
        fuelMeter.rectTransform.sizeDelta = new Vector2(
            originalSize.x, 
            originalSize.y * _fuelLevel
        );
    }
}
