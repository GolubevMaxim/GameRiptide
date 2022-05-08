using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HealthVisualiser
{
    void updateHealth(int health);
    void updateHealthMax(int healthMax);

    void Init(Transform target, int healthMax, int health);
}
