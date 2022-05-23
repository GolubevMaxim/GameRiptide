using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHandler : MonoBehaviour
{
    public static Vector3 getCorrectedZ(Vector3 position)
    {
        return new Vector3(position.x, position.y, 0.01f * position.y);
    }

    public static Vector3 getCorrectedZ(float x, float y)
    {
        return new Vector3(x, y, 0.01f * y);
    }

    void Start()
    {
        GameObject[] roomObjects = GameObject.FindGameObjectsWithTag("RoomPart");
        /*Transform[] roomTransforms = GetComponentsInChildren<Transform>();
        foreach(var transform in roomTransforms)
        {
            transform.position = getCorrectedZ(transform.position);
        }*/
        foreach(var obj in roomObjects)
        {
            obj.transform.position = getCorrectedZ(obj.transform.position);
            Debug.Log($"{obj.name} position corrected to {obj.transform.position}");
        }
    }
}
