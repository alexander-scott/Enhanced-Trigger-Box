using UnityEngine;
using System.Collections;

public class FPSControllerShoot : MonoBehaviour
{
    public GameObject cubePrefab;

    public float velocity = 1000f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = Instantiate(cubePrefab);
            go.transform.position = Camera.main.transform.position;
            Rigidbody rb = go.GetComponent<Rigidbody>();
            Vector3 v3T = Input.mousePosition;
            v3T.z = 10.0f;
            go.transform.LookAt(Camera.main.ScreenToWorldPoint(v3T));
            rb.AddRelativeForce(Vector3.forward * velocity);
        }
    }
}
