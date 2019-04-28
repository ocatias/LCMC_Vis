using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFloatingCamera : MonoBehaviour {

    float phi = -2.5f;
    float theta = 7.4f;
    public float phiVelocity = 3f;
    public float thetaVelocity = 1.5f;
    public float zoomVelocity = 15f;
    public float radius = 30;
    

	// Use this for initialization
	void Start () {
		
	}
	
    void updateCamera()
    {
        if (radius < 0)
            radius = 0;

        transform.position = new Vector3(radius*Mathf.Sin(theta)*Mathf.Cos(phi), radius * Mathf.Cos(theta), radius * Mathf.Sin(theta) * Mathf.Sin(phi));
        transform.LookAt(Vector3.zero);
    }

	// Update is called once per frame
	void Update () {
        updateCamera();

        if (Input.GetKey(KeyCode.W))
        {
            theta -= thetaVelocity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            theta += thetaVelocity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            phi -= phiVelocity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            phi += phiVelocity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            radius += zoomVelocity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            radius -= zoomVelocity * Time.deltaTime;
        }
    }
}
