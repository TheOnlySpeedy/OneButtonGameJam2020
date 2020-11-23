using UnityEngine;

namespace Interfaces
{
    public interface ICanTeleportInterface
    {
        public bool CanTeleport();

        public bool CanTeleportThisFrame();

        public void Teleport(Portal portal);
    }
}