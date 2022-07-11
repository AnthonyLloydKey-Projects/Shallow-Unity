using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraAttach : NetworkBehaviour
{
    [SerializeField] private Transform parent;

    private GameObject obj;
    private CameraController controller;

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        
        if (SceneManager.GetActiveScene().name != GlobalProperties.MultiplayerSceneName)
        {
            return;
        }
        
        if (!controller)
        {
            controller ??= FindObjectOfType<CameraController>();
            obj = controller.gameObject;
        }
        
        while (parent.childCount == 0)
        {
            controller.character = GetComponent<CharacterController>();
            controller.transform.SetParent(parent);
        }

        if (obj)
        {
            if (obj.transform.localPosition.x != 0 || obj.transform.localPosition.y != 0 ||
                obj.transform.localPosition.z != 0)
            {
                obj.transform.localPosition = Vector3.zero;
            }
        }
    }
}