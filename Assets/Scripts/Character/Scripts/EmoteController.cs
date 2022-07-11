using System.Collections;
using System.Collections.Generic;
using Pc.Character.Scripts;
using UnityEngine;

public class EmoteController : MonoBehaviour, IEmote
{
    [SerializeField] private GameObject emotePanel;
    [SerializeField] private PlayController controller;
    
    public void Show()
    {
        controller.DisableMovement();
        emotePanel.SetActive(true);
    }

    public void Hide()
    {
        controller.EnableMovement();
        emotePanel.SetActive(false);
    }
}
