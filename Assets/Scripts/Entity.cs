using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public int Health { get; protected set; }
    public int MaxHealth { get; protected set; }
    public int Damage { get; protected set; }
    public int Speed { get; protected set; }

    public bool IsStunned {  get; protected set; }
    public bool IsAlive {  get; protected set; }
    public bool IsAttacking { get; protected set; }

    protected Animator animator;

    protected abstract void Die();
    protected abstract void Attack();
    protected abstract void GetStunned();
  

    public virtual void TakeDamage(int damageTaken)
    {
        Health -= damageTaken;
    }

    

}
