using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void AnimateAgent(float speed)
    {
        SetWalkAnimation(speed > 0);
    }

    public void SetWalkAnimation(bool value)
    {
        _animator.SetBool("Walk", value);
    }

    public void StopAnimation()
    {
        _animator.speed = 0;
    }

    public void PlayAnimation()
    {
        _animator.speed = 1f;
    }
}
