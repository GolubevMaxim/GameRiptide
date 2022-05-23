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
    [SerializeField] protected ushort _networkID;
    [SerializeField] protected ushort _id;
    protected Transform _caster;
    [SerializeField] protected Room _room;

    public ushort NetworkID => _networkID;
    public ushort ID => _id;

    public void Init(Room room)
    {
        _room = room;
    }

    private void OnDestroy()
    {
        _room.DestroySpell(_networkID);
    }
}
