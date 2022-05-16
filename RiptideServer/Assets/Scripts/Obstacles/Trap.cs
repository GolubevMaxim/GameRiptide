using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Killable target = null;
        if (collision.transform.TryGetComponent<Killable>(out target))
        {
            target.ApplyDamage(damage);
        }
    }
}
