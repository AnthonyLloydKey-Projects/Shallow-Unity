using System.Collections;
using System.Collections.Generic;
using Mirror;
using Pc.Character.Scripts;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour, IAnimate
{
    private Animator animator;

    public void Initiate(Animator anim)
    {
        animator = anim;
    }
    
    public void Idle()
    {
        animator.SetBool("torchWalk", false);
        animator.SetBool("torchIdle", false);
    }

    public void TorchWalk(bool state)
    {
        animator.SetBool("torchWalk", state);
    }

    public void TorchIdle(bool state)
    {
        animator.SetBool("torchIdle", state);
    }

    public void TorchOut(bool state)
    {
        animator.SetBool("torchOut", state);
    }

    public void GrabItem()
    {
        animator.SetTrigger("pickup");
    }

    public void ClimbHide()
    {
        animator.SetTrigger("climbHide");
    }
}
