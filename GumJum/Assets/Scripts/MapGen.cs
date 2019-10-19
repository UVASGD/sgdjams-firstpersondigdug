using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    Carver carver;

    [SerializeField]
    float breadth = 1f;

    [SerializeField]
    Vector3Int mapDimens;

    static Quaternion[] orientations = {
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(90, 0, 0),
        Quaternion.Euler(0, 90, 0),
        Quaternion.Euler(0, 0, 90),
    };

    // Start is called before the first frame update
    void Start()
    {
        Allocate();

        Carve(new Vector3Int(0, 0, -5), 4, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Allocate()
    {
        Vector3 offset = new Vector3(mapDimens.x, mapDimens.y, mapDimens.z) * 0.5f;

        for (int x=0; x<mapDimens.x; x++)
        {
            for (int y=0; y<mapDimens.y; y++)
            {
                for (int z=0; z < mapDimens.z; z++)
                {
                    GameObject obj = Instantiate(prefab, transform);
                    obj.transform.localPosition = (new Vector3(x, y, z) - offset) * breadth;
                }
            }
        }
    }

    void Carve(Vector3Int center, float height, float radius)
    {
        Debug.Log("called!");
        Carver carve = Instantiate(carver, transform);
        carve.transform.localPosition = center;

        carve.transform.localRotation = orientations[Random.Range(0, orientations.Length)];

        carve.SetDimens(height * breadth, radius * breadth);
    }
}
