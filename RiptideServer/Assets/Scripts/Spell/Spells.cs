using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class Spells : MonoBehaviour
{
    private ushort _idcounter;
    private Dictionary<ushort, Spell> _updatebleSpells;
    private Dictionary<ushort, Spell> _attachedSpells;

    private void Start()
    {
        _updatebleSpells = new();
        _attachedSpells = new();
        _idcounter = 0;
    }

    public void AddUpdateble(Spell spell)
    {
        _updatebleSpells.Add(spell.NetworkID, spell);
        if (_idcounter == ushort.MaxValue) _idcounter = 0;
        else _idcounter++;        
    }

    public void AddAttached(Spell spell)
    {
        _attachedSpells.Add(spell.NetworkID, spell);
        if (_idcounter == ushort.MaxValue) _idcounter = 0;
        else _idcounter++;
    }

    public void Remove(ushort id)
    {
        _updatebleSpells.Remove(id);
        _attachedSpells.Remove(id);
    }

    //for updating spells only, not for creating
    public bool AddUpdatableToMessage(Message message)
    {
        if (_updatebleSpells.Count <= 0) return false;
        foreach(Spell spell in _updatebleSpells.Values)
        {
            message.AddUShort(spell.NetworkID);
            message.AddFloat(spell.transform.position.x);
            message.AddFloat(spell.transform.position.y);
        }
        return true;
    }

    //for new player
    public void AddSpellsToMessage(Message message)
    {
        message.AddInt(_updatebleSpells.Count);
        foreach (Spell spell in _updatebleSpells.Values)
        {
            message.AddUShort(spell.NetworkID);
            message.AddUShort(spell.ID);
            message.AddFloat(spell.transform.position.x);
            message.AddFloat(spell.transform.position.y);
        }
        foreach (Spell spell in _attachedSpells.Values)
        {
            message.AddUShort(spell.NetworkID);
            message.AddUShort(spell.ID);
        }
    }
}
