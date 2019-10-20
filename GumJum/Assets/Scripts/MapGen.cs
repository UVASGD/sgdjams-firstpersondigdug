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

    [SerializeField]
    int seams;

    [SerializeField]
    int bois;

    [SerializeField]
    int rocks;

    [SerializeField]
    GameObject rock_prefab;

    Vector3 offset;

    MapNode[,,] map;

    static Quaternion[] orientations = {
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(90, 0, 0),
        Quaternion.Euler(0, 90, 0),
        Quaternion.Euler(0, 0, 90),
    };

    void Start()
    {
        offset = new Vector3(mapDimens.x, mapDimens.y, mapDimens.z) * 0.5f;

        Allocate();
        CarveSeams();
        PlaceRocks();
    }

    void Allocate()
    {
        map = new MapNode[mapDimens.x, mapDimens.y, mapDimens.z];

        for (int x = 0; x < mapDimens.x; x++)
        {
            for (int y = 0; y < mapDimens.y; y++)
            {
                for (int z = 0; z < mapDimens.z; z++)
                {
                    GameObject obj = Instantiate(prefab, transform);
                    obj.transform.localPosition = (new Vector3(x, y, z) - offset) * breadth;

                    map[x, y, z] = obj.GetComponent<MapNode>();

                    if (x == 0 || z == 0 || y == 0 || x == mapDimens.x - 1 || z == mapDimens.z - 1)
                    {
                        map[x, y, z].Block.tag = "Untagged";
                    }
                }
            }
        }
    }

    void CarveSeams()
    {
        Vector3 position;
        int height;
        float radius;
        bool spawn_boi = false;

        for (int i = 0; i < seams; i++)
        {
            position = new Vector3(
                Random.Range(0, mapDimens.x),
                Random.Range(0, mapDimens.y),
                Random.Range(0, mapDimens.z)
                ) - offset;
            height = Random.Range(2, 4);
            radius = 0.5f;
            spawn_boi = i < bois;

            CarveSeam(
                position,
                height,
                radius,
                spawn_boi,
                orientations[Random.Range(0, orientations.Length)]
            );
        }

        CarveSeam(
            new Vector3(0f, mapDimens.y / 2, 0f),
            mapDimens.y / 2,
            radius = 0.25f,
            false,
            orientations[0]
        );
    }

    void CarveSeam(Vector3 center, float height, float radius, bool spawn_boi, Quaternion orientation)
    {
        Carver carve = Instantiate(carver, transform);
        carve.transform.localPosition = center;

        carve.transform.localRotation = orientation;

        carve.SetDimens(height * breadth, radius * breadth);

        if (spawn_boi)
            SpawnBoiAt(center);
    }

    void SpawnBoiAt(Vector3 center)
    {
        // TODO: Spawn Boi Here
        Instantiate(prefab, transform);
    }

    void PlaceRocks()
    {
        for (int i = 0; i < rocks; i++)
        {
            Vector3Int at;
            MapNode inNode;
            bool goodSpot = false;

            int tries = 0;
            while (!goodSpot)
            {
                at = new Vector3Int(
                Random.Range(1, mapDimens.x - 1),
                Random.Range(1, mapDimens.y - 1),
                Random.Range(1, mapDimens.z - 1)
                );
                inNode = map[at.x, at.y, at.z];

                if (!inNode.Broken)
                {
                    goodSpot = true;
                    //MapNode atNode = map[at.x, at.y, at.z];
                    //atNode.Break();

                    PlaceRock(at, inNode.Block);
                }

                if (tries > 50)
                {
                    Debug.Log("i screm");
                    break;

                }
                tries++;
            }
        }
    }


    void PlaceRock(Vector3Int at, Block over)
    {
        Rock rock = Instantiate(rock_prefab, transform).GetComponent<Rock>();
        rock.transform.localPosition = (at - offset) * breadth;
        rock.SetSupportBlock(over);
    }
}
