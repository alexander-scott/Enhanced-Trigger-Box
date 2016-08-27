using UnityEngine;
using System.Collections;

namespace EnhancedTriggerbox.Demo
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


