using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TargetType
{
    player,
    mobe
}

public class SpellCaster : MonoBehaviour
{
    private ushort _targetID;
    private TargetType _targetType;

    public void setTarget(ushort id, TargetType type)
    {
        _targetID = id;
        _targetType = type;
        Room.RoomNetwork.SendSpellCreateRequest(0, _targetID, _targetType);
    }

    private void Start()
    {
        GameEvents.setTargetEvent.AddListener(setTarget);
    }

    private void OnDestroy()
    {
        GameEvents.setTargetEvent.RemoveListener(setTarget);
    }
}
