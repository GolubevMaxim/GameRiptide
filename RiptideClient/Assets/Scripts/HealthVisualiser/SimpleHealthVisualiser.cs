using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleHealthVisualiser : MonoBehaviour, HealthVisualiser
{
    private int _health;
    private int _healthMax;
    private TMP_Text _text;
    private Transform _target = null;
    public void updateHealth(int health)
    {
        _health = health;
        _text.text = _health + " hp";
    }

    public void updateHealthMax(int healthMax)
    {
        _healthMax = healthMax;
    }

    public void Init(Transform target, int healthMax, int health)
    {
        _target = target;
        _text = GetComponentInChildren<Canvas>().
            GetComponentInChildren<TMP_Text>();
        updateHealthMax(healthMax);
        updateHealth(health);
    }

    

    private void Update()
    {
        if(_target != null)
        transform.position = _target.position;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
