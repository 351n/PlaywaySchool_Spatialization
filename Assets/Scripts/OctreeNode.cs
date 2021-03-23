using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeNode : MonoBehaviour
{
    Vector3 position;
    public int depth;
    public float size;
    OctreeNode parentNode;
    OctreeNode[] subNodes;

    public List<Entity> enities;

    public void Subdivide(GameObject prefab, int desiredDepth, GameObject entityPrefab) {
        float val = (size / 4);

        if(IsLeaf()) {
            if(depth < desiredDepth) {
                for(int i = 0; i < 8; i++) {
                    Vector3 desiredPos = new Vector3();
                    switch(i) {
                        case 0:
                            desiredPos = position + new Vector3(-val, +val, -val);
                            break;
                        case 1:
                            desiredPos = position + new Vector3(-val, +val, +val);
                            break;
                        case 2:
                            desiredPos = position + new Vector3(+val, +val, -val);
                            break;
                        case 3:
                            desiredPos = position + new Vector3(+val, +val, +val);
                            break;
                        case 4:
                            desiredPos = position + new Vector3(-val, -val, -val);
                            break;
                        case 5:
                            desiredPos = position + new Vector3(-val, -val, +val);
                            break;
                        case 6:
                            desiredPos = position + new Vector3(+val, -val, -val);
                            break;
                        case 7:
                            desiredPos = position + new Vector3(+val, -val, +val);
                            break;
                    }

                    subNodes[i] = Instantiate(prefab, desiredPos, Quaternion.identity, this.transform).GetComponent<OctreeNode>();
                    subNodes[i].Initialize(depth + 1, this, desiredPos, size / 2);
                }

                foreach(OctreeNode n in subNodes) {
                    n.Subdivide(prefab, desiredDepth, entityPrefab);
                }
            } else {
                var entity = Instantiate(entityPrefab, position, Quaternion.identity);
                entity.GetComponent<Entity>().node = this;
                this.enities.Add(entity.GetComponent<Entity>());
            }
        }
    }

    public void Initialize(int depth, OctreeNode parent, Vector3 position, float size) {
        this.position = position;
        this.depth = depth;
        this.size = size;
        this.parentNode = parent;
        subNodes = new OctreeNode[8];
        this.name = $"Node {depth} {GetNodePosition()}";
    }

    public bool IsRoot() {
        if(parentNode) {
            return false;
        } else {
            return true;
        }
    }

    public bool IsLeaf() {
        if(subNodes[0]) {
            return false;
        } else {
            return true;
        }
    }

    public NodePosition GetNodePosition() {
        int resul = 0;

        if(!IsRoot()) {
            if(position.y < parentNode.position.y) {
                resul += 1;
            }

            if(position.x > parentNode.position.x) {
                resul += 2;
            }

            if(position.z > parentNode.position.z) {
                resul += 4;
            }
        }

        return (NodePosition)resul;
    }

    internal List<Entity> GetEnitiesInNeighborhood() {
        if(!IsRoot()) {
            return parentNode.GetEntitiesInSubNodes();
        } else {
            return null;
        }
    }

    internal List<Entity> GetEnitiesInNode() {
        return enities;
    }

    internal List<Entity> GetEnitiesInSubNode(NodePosition position) {
        if(!IsLeaf()) {
            return subNodes[(int)position].GetEnitiesInNode();
        } else {
            return null;
        }
    }

    internal List<Entity> GetEntitiesInSubNodes() {
        List<Entity> result = new List<Entity>();

        if(!IsLeaf()) {
            foreach(OctreeNode n in subNodes) {
                result.AddRange(n.GetEnitiesInNode());
            }
            return result;
        } else {
            return null;
        }
    }
}
