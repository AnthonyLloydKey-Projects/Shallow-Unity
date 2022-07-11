using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Pc.Character.Scripts;
using UnityEngine;

public class TorchController : NetworkBehaviour, ITorch
{
    [SerializeField] private GameObject torchObj;
    [SerializeField] private float batteryPercentage = 100f;
    
    private CharacterController controller;
    [SyncVar(hook = nameof(CmdTorch))]
    private bool torchSwitch;

    public float BatteryPercentage => batteryPercentage;
    
    private void Update()
    {
        if (!controller)
        {
            return;
        }
        
        if (!controller.IsWalking() && torchSwitch)
        {
            controller.GetAnimations().TorchIdle(true);
            controller.GetAnimations().TorchWalk(false);
        }
        
        if (controller.IsWalking() && torchSwitch)
        {
            controller.GetAnimations().TorchWalk(true);
            controller.GetAnimations().TorchIdle(false);
        }

        if (!torchSwitch)
        {
            //todo add logic to only fire this once
            controller.GetAnimations().Idle();
        }
    }

    private void CmdTorch(bool old, bool newv)
    {
        if (isServer)
        {
            torchObj.SetActive(newv);
        }

        if (isClient)
        {
            torchObj.SetActive(newv);
        }
    }

    [Command]
    public void CmdToggle(CharacterController character)
    {
        if (!controller)
        {
            controller = character;
        }

        var previous = torchSwitch;
        
        if (!torchSwitch)
        {
            controller.GetAnimations().TorchOut(false);
            torchObj.SetActive(true);
            torchSwitch = true;
        }
        else
        {
            controller.GetAnimations().TorchOut(true);
            torchObj.SetActive(false);
            torchSwitch = false;
        }
        
        CmdTorch(previous, torchSwitch);
    }
}