using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public OctreeNode node;
    float radius = 1f;

    public void Update() {
        CheckForCollisions(node.GetEnitiesInNeighborhood());
    }

    private bool CheckForCollisions(List<Entity> entities) {
        if(entities.Count > 0) {
            foreach(Entity e in entities) {
                if(Vector3.Distance(transform.position, e.transform.position) <= radius + e.radius) {
                    return true;
                }
            }
            return false;
        } else {
            return false;
        }
    }
}
