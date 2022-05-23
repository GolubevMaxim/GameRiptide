using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class RoomSpells : MonoBehaviour
{
    private Spells _updatebleSpells;
    private Spells _attachedSpells;

    private void Start()
    {
        _updatebleSpells = new();
        _attachedSpells = new();
    }

    
}
