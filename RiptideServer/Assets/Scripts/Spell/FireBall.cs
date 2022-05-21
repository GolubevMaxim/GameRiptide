using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Spell
{
    private Transform _target;
    private float _speed = 2;
    private int _dmg = 50;
    private int _cost = 10;

    public void Init(Transform caster, Transform target, Rooms.Room room)
    {
        _caster = caster;
        _target = target;
        _room = room;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.TryGetComponent<Killable>(out Killable victim))
        {
            victim.ApplyDamage(_dmg);
        }
        Destroy(gameObject);
    }
}
