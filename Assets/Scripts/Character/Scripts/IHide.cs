using UnityEngine;

namespace Pc.Character.Scripts
{
    public abstract class IHide : MonoBehaviour
    {
        public Transform cameraPos;
        
        public abstract void Enter(CharacterController character);
    }
}