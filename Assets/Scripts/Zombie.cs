using UnityEngine;

public class Zombie : Enemy
{
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsRunning", true);
    }
    private void OnTriggerEnter(Collider other)
    {
        Attack();
    }
    private void OnTriggerExit(Collider other)
    {
        Run();
    }
    private void Update()
    {

    }
    protected override void Die()
    {

    }
    protected override void Attack()
    {
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsRunning", false);
    }
    protected override void GetStunned()
    {

    }
    protected override void Run()
    {
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttacking", false);
    }
    
}
