using System;
using Pc.Scripts.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pc.Character.Scripts
{
    public class InteractionController : MonoBehaviour, IInteract
    {
        private Scanner scanner;

        private void Update()
        {
            if (SceneManager.GetActiveScene().name != GlobalProperties.MultiplayerSceneName)
            {
                return;
            }

            if (!scanner)
            {
                scanner ??= FindObjectOfType<Scanner>();
            }
        }

        public void Pickup(CharacterController controller)
        {
            if (scanner.GetScan())
            {
                var scanned = scanner.GetScan().gameObject;

                if (scanned.GetComponent<IPickup>())
                {
                    scanned.GetComponent<IPickup>().Pickup();
                    controller.GetAnimations().GrabItem();
                }
                
                if (scanned.GetComponent<IHide>())
                {
                    scanned.GetComponent<IHide>().Enter(controller);
                }
                
            }
        }
    }
}