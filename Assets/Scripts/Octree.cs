using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject entityPrefab;
    public OctreeNode node;
    private int depth = 2;
    private float size = 128;

    public void Start() {
        SpawnNodes();
    }

    public void SpawnNodes() {
        var spawnedNode = Instantiate(nodePrefab);
        node = spawnedNode.GetComponent<OctreeNode>();
        node.Initialize(0, null, new Vector3(size / 2, size / 2, size / 2), size);
        node.Subdivide(nodePrefab, 5, entityPrefab);
    }
}

public enum NodePosition    //yxz
{
    UpperLeftFront = 0,     //000
    UpperRightFront = 2,    //010
    UpperRightBack = 3,     //011
    UpperLeftBack = 1,      //001
    BottomLeftFront = 4,    //100
    BottomRightFront = 6,   //110
    BottomRightBack = 7,    //111
    BottomLeftBack = 5      //101
}
