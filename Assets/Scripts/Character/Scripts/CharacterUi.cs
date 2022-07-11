using System;
using System.Collections;
using System.Collections.Generic;
using Pc.Scripts.Core;
using UnityEngine;

public class CharacterUi : MonoBehaviour
{
    [SerializeField] private GameObject pickUpUi;

    private Scanner scanner;

    private void Start()
    {
        scanner ??= FindObjectOfType<Scanner>();
    }

    private void Update()
    {
        if (scanner.GetScan())
        {
            if (scanner.GetScan().GetComponent<IPickup>())
            {
                pickUpUi.SetActive(true);
            }
            else
            {
                pickUpUi.SetActive(false);
            }
        }
    }
}
