using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

	// Use this for initialization
	void Start () {
		if(cam == null) {
            Debug.LogError("PlayerShoot: No camera referenced.");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();

    }
	
	// Update is called once per frame
	void Update () {

        currentWeapon = weaponManager.GetCurrentWeapon();
        if (!isLocalPlayer) {

        }

        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
	}

    [Client]
    private void Shoot() {
        RaycastHit _hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask)) {
            if(_hit.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
            }
        }
    }

    [Command]
    private void CmdPlayerShot(string _playerID, int _damage) {
        Debug.Log(_playerID + " has been shot");
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }
}
