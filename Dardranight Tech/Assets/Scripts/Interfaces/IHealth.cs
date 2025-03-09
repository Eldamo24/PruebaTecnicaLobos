using UnityEngine;

public interface IHealth
{
    float MaxHealth { get; set; }
    float Health { get; set; }

    public void TakeDamage(float damage);
    void Heal(float amount);
    void Die();
}
