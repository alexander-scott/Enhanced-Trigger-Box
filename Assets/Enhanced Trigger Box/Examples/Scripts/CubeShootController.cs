using UnityEngine;
using System.Collections;

namespace EnhancedTriggerbox.Examples
{
    public class CubeShootController : MonoBehaviour
    {
        public void ShootCubes(float velocity)
        {
            foreach (var c in GetComponentsInChildren<CubeShoot>())
            {
                c.ShootCube(velocity);
            }
        }
    }
}


