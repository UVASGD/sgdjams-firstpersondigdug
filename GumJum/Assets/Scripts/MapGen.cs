using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public static MapGen instance;

    [SerializeField]
    GameObject prefab;

    [SerializeField]
    Carver carver;

    [SerializeField]
    float breadth = 1f;

    [SerializeField]
    Vector3Int mapDimens;

    [SerializeField]
    int averageSeamSize = 3;
    int numseams;

    int boiCount;
    [SerializeField]
    GameObject[] bois;

    [HideInInspector]
    public int boisSpawned;

    [SerializeField]
    GameObject pineapple;

    [SerializeField]
    GameObject bounder;

    int rocks;

    [SerializeField]
    GameObject rockPrefab;

    Vector3 offset;

    public MapNode[,,] map;

    static Quaternion[] orientations = {
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(90, 0, 0),
        Quaternion.Euler(0, 90, 0),
        Quaternion.Euler(0, 0, 90),
    };

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        boisSpawned = 0;
    }

    void Start()
    {
        int currentlevel = GameManager.Instance.level;
        float widthScaleFactor = Mathf.Max(1f, 0.7f * Mathf.Pow(currentlevel, 1f / 3f));

        mapDimens += new Vector3Int(1, 1, 1) * (currentlevel * 2);

        transform.position -= new Vector3(0f, mapDimens.y / 2f, 0f) * breadth;

        numseams = 3 + currentlevel;

        rocks = (mapDimens.x + currentlevel) * 2;

        print("currentlevel: " + currentlevel);
        print("widthScaleFactor: " + widthScaleFactor);
        print("numseams: " + numseams);

        offset = new Vector3(mapDimens.x, mapDimens.y, mapDimens.z) * 0.5f;

        Debug.Log("offset: " + offset.ToString());

        Allocate();
        CarveSeams();

        PlaceRocks();

        if (bounder)
        {
            GameObject bound1 = Instantiate(bounder, transform);

            bound1.transform.localScale = new Vector3(mapDimens.x, mapDimens.y * 2f, mapDimens.z) * breadth;
            bound1.transform.localPosition = new Vector3(0f, mapDimens.y, 0f) * breadth;
            bound1.transform.localPosition -= new Vector3(breadth / 2f, 0f, breadth / 2f);
        }
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



                }
            }
        }
    }

    enum Spawn { None, Boi, Pineapple }

    void CarveSeams()
    {
        Vector3 position;
        int height;
        float radius;
        Spawn spawnOpt;

        for (int i = 0; i < numseams; i++)
        {
            position = new Vector3(
                Random.Range(0, mapDimens.x),
                Random.Range(0, mapDimens.y),
                Random.Range(0, mapDimens.z)
                ) - offset;
            height = Random.Range(averageSeamSize - 1, averageSeamSize + 1);
            radius = 0.5f;

            if (i == 0)
                spawnOpt = Spawn.Pineapple;
            else
                spawnOpt = Spawn.Boi;

            CarveSeam(
                position,
                height,
                radius,
                spawnOpt,
                orientations[Random.Range(0, orientations.Length)]
            );
        }

        CarveSeam(
            new Vector3(0f, mapDimens.y / 2, 0f),
            mapDimens.y / 2,
            radius = 0.5f,
            Spawn.None,
            orientations[0]
        );
    }

    void CarveSeam(Vector3 center, float height, float radius, Spawn spawnOpt, Quaternion orientation)
    {
        Carver carve = Instantiate(carver, transform);
        carve.transform.localPosition = center;

        carve.transform.localRotation = orientation;

        carve.SetDimens(height * breadth, radius * breadth);

        if (spawnOpt == Spawn.Boi)
        {
            //SpawnBoiAt(center);
        }
        else if (spawnOpt == Spawn.Pineapple)
        {
            SpawnPineappleAt(center);
        }
    }

    void SpawnBoiAt(Vector3 center)
    {
        /*int index = bois.Length - 1;
        if (boisSpawned < Mathf.CeilToInt(boiCount * 2 / 3))
        {
            index = Random.Range(0, bois.Length - 1);
        }*/

        int index = Random.Range(0, bois.Length - 1);

        boisSpawned++;
        GameObject boi = Instantiate(bois[index], transform.position + center, Quaternion.identity);
    }

    void SpawnPineappleAt(Vector3 center)
    {
        if (pineapple)
            Instantiate(pineapple, transform.position + center, Quaternion.identity);
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
        if (rockPrefab)
        {
            Rock rock = Instantiate(rockPrefab, transform).GetComponent<Rock>();
            rock.transform.localPosition = (at - offset) * breadth;
            rock.SetSupportBlock(over);
        }
    }
}
