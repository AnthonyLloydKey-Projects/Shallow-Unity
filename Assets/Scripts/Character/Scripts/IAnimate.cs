using UnityEngine;

namespace Pc.Character.Scripts
{
    public interface IAnimate
    {
        public void Initiate(Animator anim);

        public void Idle();
        public void TorchWalk(bool state);
        public void TorchIdle(bool state);

        public void TorchOut(bool state);
        public void GrabItem();
        public void ClimbHide();
    }
}