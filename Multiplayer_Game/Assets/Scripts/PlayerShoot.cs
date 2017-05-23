using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

	// Use this for initialization
	void Start () {
		if(cam == null) {
            Debug.LogError("PlayerShoot: No camera referenced.");
            this.enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) {

        }

        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
	}

    private void Shoot() {
        RaycastHit _hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask)) {
            Debug.Log("We hit " + _hit.collider.name);
        }
    }
}
