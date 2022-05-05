using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleHealthVisualiser : MonoBehaviour, HealthVisualiser
{
    private int _health;
    private int _healthMax;
    private TMP_Text text;
    public void updateHealth(int health)
    {
        _health = health;
        text.text = _health + " hp";
    }

    public void updateHealthMax(int healthMax)
    {
        _healthMax = healthMax;
    }

    public void Init(int healthMax, int health)
    {
        text = GetComponentInChildren<Canvas>().
            GetComponentInChildren<TMP_Text>();
        updateHealthMax(healthMax);
        updateHealth(health);
    }
}
