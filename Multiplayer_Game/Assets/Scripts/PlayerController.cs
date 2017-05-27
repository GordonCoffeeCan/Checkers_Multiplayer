using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3;

    [SerializeField]
    private float thrusterForce = 1000;

    [SerializeField]
    private float thrusterFuelBurnSpeed = 1;

    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1;

    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    [SerializeField]
    private LayerMask evironmentMask;

    [Header("Spring Settings:")]
    
    [SerializeField]
    private float jointSpring = 20;
    [SerializeField]
    private float jointMaxForce = 40;

	// Use this for initialization
	void Start () {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();

        SetJointSettings(jointSpring);
    }
	
	// Update is called once per frame
	void Update () {

        if (PauseMenu.IsOn) {
            return;
        }

        //Setting target postion for spring. This makes the physics act right when it comes to applying gravity when flying over ojbects
        RaycastHit _hit;
        if(Physics.Raycast(transform.position, Vector3.down, out _hit, 100, evironmentMask)) {
            joint.targetPosition = new Vector3(0, -_hit.point.y, 0);
        } else {
            joint.targetPosition = new Vector3(0, 0, 0);
        }

        //Calculate movement velocity as a 3d vector

        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

        //Animate movement
        animator.SetFloat("ForwardVelocity", _zMov);

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
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0) {

            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if(thrusterFuelAmount >= 0.01f) {
                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(0);
            }

        } else {

            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;

            SetJointSettings(jointSpring);
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0, 1);

        //Apply the thruster force
        motor.ApplyThruster(_thrusterForce);
    }

    private void SetJointSettings(float _jointSpring) {
        joint.yDrive = new JointDrive {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };

    }

    public float GetThrusterFuelAmount() {
        return thrusterFuelAmount;
    }
}
