using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    float breadth = 1f;

    [SerializeField]
    Vector3Int mapDimens;

    GameObject[,,] map;

    // Start is called before the first frame update
    void Start()
    {
        Allocate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Allocate()
    {
        map = new GameObject[mapDimens.x, mapDimens.y, mapDimens.z];

        for (int x=0; x<mapDimens.x; x++)
        {
            for (int y=0; y<mapDimens.y; y++)
            {
                for (int z=0; z < mapDimens.z; z++)
                {
                    GameObject obj = Instantiate(prefab, transform);
                    map[x, y, z] = obj;

                    obj.transform.localPosition = new Vector3(x, y, z) * breadth;
                }
            }
        }
    }
}
