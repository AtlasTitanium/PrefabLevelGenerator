using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public List<Transform> doors = new List<Transform>();
    public List<BoxCollider> colliders = new List<BoxCollider>();

    public bool CheckRoom(Vector3 newPositon, LayerMask layer) {
        foreach(BoxCollider col in colliders) {
            Vector3 pos = newPositon + col.center;
            if(Physics.CheckBox(pos, col.size/2, transform.rotation, layer)) {
                return true;
            }
        }
        return false;
    }
}
