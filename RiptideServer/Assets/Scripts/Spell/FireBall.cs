using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Spell
{
    private Transform _target;
    [SerializeField] private float _speed = 12;
    private int _dmg = 50;
    private int _cost = 10;
    private Rigidbody2D rgbd;

    public void Init(Transform caster, Transform target, Rooms.Room room, ushort networkID)
    {
        _caster = caster;
        _target = target;
        _room = room;
        Collider2D collider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider, caster.GetComponent<Collider2D>());
        collider.enabled = true;
        rgbd = GetComponent<Rigidbody2D>();
        _networkID = networkID;
    }

    private void Update()
    {
        if(_target != null) rgbd.velocity = (_target.transform.position - transform.position).normalized * _speed;
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
