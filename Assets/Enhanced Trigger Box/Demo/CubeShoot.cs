using UnityEngine;
using System.Collections;

public class CubeShoot : MonoBehaviour {

	// Update is called once per frame
	public void ShootCube (float velocity) {
        var playerpos = Camera.main.transform.position;
        var direction = (playerpos - transform.position) * velocity;
        GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
	}
}
