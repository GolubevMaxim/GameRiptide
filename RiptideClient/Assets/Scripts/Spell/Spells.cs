using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    private Dictionary<ushort, Spell> _dictionary;
    public Dictionary<ushort, Spell> Dictionary => _dictionary;
    private void Start()
    {
        _dictionary = new();
    }
}
