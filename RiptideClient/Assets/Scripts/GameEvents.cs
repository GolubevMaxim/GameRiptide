using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetTargetEvent : UnityEvent<ushort, TargetType>
{
}

public class GameEvents : MonoBehaviour
{
    public static SetTargetEvent setTargetEvent = new SetTargetEvent();
}
