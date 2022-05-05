using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HealthVisualiser
{
    void updateHealth(int health);
    void updateHealthMax(int healthMax);

    void Init(int healthMax, int health);
}
