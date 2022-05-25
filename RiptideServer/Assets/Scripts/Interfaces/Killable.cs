using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Killable
{
    void ApplyDamage(int damage);
    void ApplyHeal(int heal);
    void Die();
}
