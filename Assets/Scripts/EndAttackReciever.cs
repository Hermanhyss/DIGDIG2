using UnityEngine;

public class EndAttackReciever : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void EndAttack()
    {
        animator.SetBool("IsAttacking", false);
    }
}
