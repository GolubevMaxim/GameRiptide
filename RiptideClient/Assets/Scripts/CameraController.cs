using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private float velocityMultiplier = 2f;
    private Vector3 velocity;

    void Update()
    {
        if (Target == null) return;

        velocity = new Vector3(Target.position.x - transform.position.x, Target.position.y - transform.position.y, 0);
        transform.position += velocity * Time.deltaTime * velocityMultiplier;
        
        /*var pos = Target.position;
        pos.z = -10;
        
        transform.position = pos;*/
    }
}
