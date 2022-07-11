using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public CharacterController character;

    [SerializeField] private float sensitivity;
    [SerializeField] float clampAngle = 85;

    private float verticalRotation;
    private float horizontalRotation;

    private void Start()
    {
        verticalRotation = transform.localEulerAngles.x;
        horizontalRotation = transform.localEulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Look();
    }

    private void Look()
    {
        float mouseVert = -Input.GetAxis("Mouse Y");
        float mouseHor = Input.GetAxis("Mouse X");

        verticalRotation += mouseVert * sensitivity * Time.deltaTime;
        horizontalRotation += mouseHor * sensitivity * Time.deltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);
        
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f,0f);
        character.transform.rotation = Quaternion.Euler(0f,horizontalRotation, 0f);
    }
}
