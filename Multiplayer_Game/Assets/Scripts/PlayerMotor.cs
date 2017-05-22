using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0;
    private float currentCameraRotationX = 0;
    private Vector3 thrusterForce = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimit = 85;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Move(Vector3 _velocity) {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation) {
        rotation = _rotation;
    }

    public void RotateCamera(float _cameraRotationX) {
        cameraRotationX = _cameraRotationX;
    }

    //Get a force vector for our thrusters
    public void ApplyThruster(Vector3 _thrusterForce) {
        thrusterForce = _thrusterForce;
    }

    private void FixedUpdate() {
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement() {
        if (velocity != Vector3.zero) {
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }

        if(thrusterForce != Vector3.zero) {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    private void PerformRotation() {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if(cam != null) {
            //cam.transform.Rotate(-cameraRotation);
            //Set our rotation and clamp it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            //Apply roation to the transform of the camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
        }

    }
}
