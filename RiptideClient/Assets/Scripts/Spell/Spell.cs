using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    private ushort _networkID;
    private ushort _id;
    private Vector3 _targetPosition;
    [SerializeField] private float interpolationCoefficient = 8f;

    public ushort NetworkID => _networkID;

    public void Init(ushort networkID, ushort id)
    {
        _networkID = networkID;
        _id = id;
        _targetPosition = transform.position;
    }

    public void UpdatePosition(float x, float y)
    {
        _targetPosition = LayerHandler.getCorrectedZ(x, y);
    }

    private void Update()
    {
        var direction = _targetPosition - transform.position;

        transform.eulerAngles = Rotate(direction.x, direction.y);

        transform.position = direction * Time.deltaTime * interpolationCoefficient + LayerHandler.getCorrectedZ(transform.position);
    }

    private static Vector3 Rotate(float x, float y)
    {
            return new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(y, x));
    }

    public void Destroy()
    {
        GetComponent<Animator>().SetBool("toBeDestroyed", true);
    }
}
