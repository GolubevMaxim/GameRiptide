using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float velocityMultiplier = 2f;
    private Vector3 _velocity;

    private Vector3 TargetPosition => target.position;
    private Vector3 CameraPosition => transform.position;

    private void Update()
    {
        if (target == null) return;

        _velocity = new Vector3(TargetPosition.x - CameraPosition.x, TargetPosition.y - CameraPosition.y, 0);
        transform.position += _velocity * (Time.deltaTime * velocityMultiplier);
    }
}
