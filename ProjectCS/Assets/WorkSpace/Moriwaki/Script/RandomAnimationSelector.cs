using UnityEngine;

public class RandomAnimationSelector : StateMachineBehaviour
{
    public int animationCount = 3; // �A�j���[�V�����̐�

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        int randomIndex = Random.Range(0, animationCount);
        animator.SetInteger("RandomIndex", randomIndex);
    }
}