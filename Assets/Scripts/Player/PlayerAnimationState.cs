using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationState : MonoBehaviour
{
    public enum AnimationState
    {
        Idle = 0,
        Walk,
        Dash,
        AttackUp,
        AttackDown,
        Dead
    }

    public AnimationState State { get; private set; }
    private Animator m_animator;

    private void OnValidate()
    {
        m_animator = GetComponent<Animator>();
    }
    
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void TransitToAnimationState(AnimationState newState)
    {
        if (newState == State || !m_animator)
            return;

        AnimationState currState = State;

        switch (newState)
        {
            case AnimationState.Idle:
                m_animator.SetBool("IsMoving", false);
                m_animator.speed = 1f;
                break;
            case AnimationState.Walk:
                m_animator.SetBool("IsMoving", true);
                m_animator.speed = 1f;
                break;
            case AnimationState.Dash:
                m_animator.SetBool("IsMoving", true);
                m_animator.speed = 2f;
                break;
            case AnimationState.AttackUp:
                break;
            case AnimationState.AttackDown:
                break;
            case AnimationState.Dead:
                m_animator.SetBool("IsMoving", false);
                m_animator.SetBool("IsDead", true);
                break;
            default:
                break;
        }

        State = currState;
    }
}
