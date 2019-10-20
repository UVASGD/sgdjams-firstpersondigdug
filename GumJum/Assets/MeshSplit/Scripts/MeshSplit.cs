using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class MeshSplit : MonoBehaviour
{
    [SerializeField] private GameObject intactMesh;
    [SerializeField] private GameObject splitMesh;

    [SerializeField] private float meshPieceMass;
    [SerializeField] private float splitForce;
    [SerializeField] private float splitRadius;
    [SerializeField] private float secondsBeforeCleanup;
    [SerializeField] private float splitY;
    [SerializeField] private float intactMeshMass;
    private List<GameObject> meshPieces;

    // Start is called before the first frame update
    void Awake()
    {
        PopulateSplitMesh();
        setMass(intactMeshMass);
        AddCollidersToSplit();
        MakeCollidersConvex();
    }

    private void setMass(float mass)
    {
        this.GetComponent<Rigidbody>().mass = mass;
    }

    void PopulateSplitMesh()
    {
        meshPieces = new List<GameObject>();
        for (int i = 0; i < splitMesh.transform.childCount; i++)
        {
            meshPieces.Add(splitMesh.transform.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        this.GetComponent<Rigidbody>().mass = intactMeshMass;
    }

    private void FixedUpdate()
    {
        // if interacting with mesh, split mesh
        // test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SplitMesh();
        }
    }

    void SplitMesh()
    {
        GameObject splitMeshClone = Instantiate(splitMesh, transform.position, transform.rotation);
        Destroy(this.gameObject);
        splitMeshClone.GetComponentInChildren<Rigidbody>().AddExplosionForce(splitForce, transform.position,
            splitRadius, splitY, ForceMode.Impulse); // determined by user
        Destroy(splitMeshClone, secondsBeforeCleanup);
    }

    void AddCollidersToSplit()
    {
        foreach (GameObject meshPiece in meshPieces)
        {
            if (meshPiece.GetComponent<Rigidbody>() == null)
            {
                meshPiece.AddComponent<Rigidbody>();
                meshPiece.GetComponent<Rigidbody>().mass = meshPieceMass;
            }
        }
    }

    void MakeCollidersConvex()
        {
            foreach (GameObject meshPiece in meshPieces)
            {
                if (meshPiece.GetComponent<MeshCollider>() == null)
                {
                    meshPiece.AddComponent<MeshCollider>();
                    meshPiece.GetComponent<MeshCollider>().convex = true;
                }
            }
        }
    }
