using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    void Update()
    {
        if (Target == null) return;
        
        var pos = Target.position;
        pos.z = -10;
        
        transform.position = pos;
    }
}
