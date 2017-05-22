using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerController))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3;

    [SerializeField]
    private float thrusterForce = 1000;


    private PlayerMotor motor;
    private ConfigurableJoint joint;

    [Header("Spring Settings:")]
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20;
    [SerializeField]
    private float jointMaxForce = 40;

	// Use this for initialization
	void Start () {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);
    }
	
	// Update is called once per frame
	void Update () {
        //Calculate movement velocity as a 3d vector

        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

        motor.Move(_velocity);

        //Calculate rotation as a 3D vector (turning character arround)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0, _yRot, 0) * lookSensitivity;

        //Apply rotation
        motor.Rotate(_rotation);

        //Calculate Camera rotation as a 3D vector (turning character arround)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        //Apply Camera rotation
        motor.RotateCamera(_cameraRotationX);

        Vector3 _thrusterForce = Vector3.zero;

        
        //Calculate the thrusterforce base on player input
        if (Input.GetButton("Jump")) {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0);
        } else {
            SetJointSettings(jointSpring);
        }

        //Apply the thruster force
        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring) {
        joint.yDrive = new JointDrive {
            mode = jointMode,
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };

    }
}
