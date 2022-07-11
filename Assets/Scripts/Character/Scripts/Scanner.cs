using System.Collections;
using System.Collections.Generic;
using Pc.Scripts.Core;
using Unity.VisualScripting;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private float distance = 5f;
    
    [SerializeField] GameObject scannedItem = null;
    
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance, mask))
        {
            scannedItem = hit.transform.gameObject;
        }
        else
        {
            scannedItem = null;
        }
    }

    public GameObject GetScan()
    {
        return scannedItem;
    }
}
