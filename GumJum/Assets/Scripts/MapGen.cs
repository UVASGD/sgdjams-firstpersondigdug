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
    float seamDensityLevel1 = 0.1f;
    [SerializeField]
    float seamDensityLevel10 = 0.25f;

    [SerializeField]
    int averageSeamSize = 3;//3 blocks
    int numseams;

    [SerializeField]
    float additionalBoiChanceLevel1 = 0f;
    [SerializeField]
    float additionalBoiChanceLevel10 = 0.4f;

    float additionalBoiSpawnChance;


    int boiCount;
    [SerializeField]
    GameObject[] bois;
    [HideInInspector]
    public int boisSpawned;



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
        mapDimens.x = (int)(mapDimens.x * widthScaleFactor);
        mapDimens.y = (int)(mapDimens.y * widthScaleFactor);
        mapDimens.z = (int)(mapDimens.z * widthScaleFactor);
        int volume = mapDimens.x * mapDimens.y * mapDimens.z;
        float levelFraction = (float)(currentlevel - 1) / (10 - 1);
        float seamDensity = Mathf.Lerp(seamDensityLevel1, seamDensityLevel10, levelFraction);
        additionalBoiSpawnChance = Mathf.Lerp(additionalBoiChanceLevel1, additionalBoiChanceLevel10, levelFraction);
        numseams = (int)(volume * seamDensity / averageSeamSize);

        print("currentlevel: " + currentlevel);
        print("widthScaleFactor: " + widthScaleFactor);
        print("volume: " + volume);
        print("levelFraction: " + levelFraction);
        print("seamDensity: " + seamDensity);
        print("additionalBoiSpawnChance: " + additionalBoiSpawnChance);
        print("numseams: " + numseams);

        offset = new Vector3(mapDimens.x, mapDimens.y, mapDimens.z) * 0.5f;

        Allocate();
        CarveSeams();
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

    void CarveSeams()
    {
        Vector3 position;
        int height;
        float radius;
        bool spawnBoi;

        for (int i = 0; i < numseams; i++)
        {
            position = new Vector3(
                Random.Range(0, mapDimens.x),
                Random.Range(0, mapDimens.y),
                Random.Range(0, mapDimens.z)
                ) - offset;
            height = Random.Range(averageSeamSize - 1, averageSeamSize + 1);
            radius = 0.5f;
            spawnBoi = true;

            CarveSeam(
                position,
                height,
                radius,
                spawnBoi,
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

    void CarveSeam(Vector3 center, float height, float radius, bool spawnBoi, Quaternion orientation)
    {
        Carver carve = Instantiate(carver, transform);
        carve.transform.localPosition = center;

        carve.transform.localRotation = orientation;

        carve.SetDimens(height * breadth, radius * breadth);

        if (spawnBoi)
        {
            SpawnBoiAt(center);
            float dice = Random.value;
            if (dice < additionalBoiSpawnChance)
            {
                SpawnBoiAt(center);
            }
        }
    }

    void SpawnBoiAt(Vector3 center)
    {
        print("Spawn boi!!!");
        int index = bois.Length - 1;
        if (boisSpawned < Mathf.CeilToInt(boiCount * 2 / 3))
        {
            index = Random.Range(0, bois.Length - 1);
        }

        boisSpawned++;
        GameObject boi = Instantiate(bois[index], center, Quaternion.identity);
        print(boi.name + ", " + boi.transform.position);
    }
}
