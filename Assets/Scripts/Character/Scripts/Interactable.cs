using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class Interactable : MonoBehaviour
{
    public Vector3 spawnSize1;
    
    [Header("Idle")]
    public Vector3 spawnPos1;
    public Vector3 spawnRot1;
    
    [Header("Walking")]
    public Vector3 spawnPos2;
    public Vector3 spawnRot2;

    [FormerlySerializedAs("SetScale")] public bool SetScaleInEditor = false;
    
    private void Update()
    {
        //transform.LookAt(2 * transform.position - new Vector3(transform.position.x, transform.position.y, transform.position.z + 5));

        if (SetScaleInEditor)
        {
            spawnSize1 = transform.localScale;
            SetScaleInEditor = false;
        }
    }

    private void Start()
    {
        spawnSize1 = transform.localScale;
    }
}
