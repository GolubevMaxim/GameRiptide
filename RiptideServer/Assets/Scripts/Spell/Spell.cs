using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rooms;

public enum SpellEffect: ushort
{
    damage,
}

public class Spell : MonoBehaviour
{
    [SerializeField] private ushort _networkID;
    [SerializeField] private ushort _id;
    protected Transform _caster;
    [SerializeField] protected Room _room;

    public void Init(Room room)
    {
        _room = room;
    }

    private void OnDestroy()
    {
        _room.DestroySpell(_networkID);
    }
}
