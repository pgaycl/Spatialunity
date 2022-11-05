using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialSys.UnitySDK
{
    [RequireComponent(typeof(Collider))]
    public class SpatialAvatarTeleporter : SpatialComponentBase
    {
        public override string prettyName => "Avatar Teleporter";
        public override string tooltip => "When an avatar enters the trigger area teleport them to the target location.";

        public Transform targetLocation;

        private void OnDrawGizmos()
        {
            if (targetLocation != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, targetLocation.position);
            }
        }
    }
}