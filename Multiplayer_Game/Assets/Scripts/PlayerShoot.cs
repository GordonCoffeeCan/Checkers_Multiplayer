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

        if (PauseMenu.IsOn) {
            return;
        }

        if(currentWeapon.fireRate <= 0) {
            if (Input.GetButtonDown("Fire1")) {
                Shoot();
            }
        } else {
            if (Input.GetButtonDown("Fire1")) {
                InvokeRepeating("Shoot", 0, 1/currentWeapon.fireRate);
            } else if(Input.GetButtonUp("Fire1")) {
                CancelInvoke("Shoot");
            }
        }

        
	}

    //Is called on the server when a player shoots
    [Command]
    private void CmdOnShoot() {
        RpcDoShootEffect();
    }

    //Is called on all clients when we need to do a shoof effect
    [ClientRpc]
    void RpcDoShootEffect() {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    //Is called on the server when we hit something
    //Takes in the hit point and the normal of the surface
    [Command]
    private void CmdOnHit(Vector3 _pos, Vector3 _normal) {
        RpcDoHitEffect(_pos, _normal);
    }

    //Is called on all clients, here we can spawn cool effects
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal) {
        GameObject _hitEffect = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 2);
    }

    [Client]
    private void Shoot() {

        if (!isLocalPlayer) {
            return;
        }

        //We are shooting, call the OnShoot method on the server
        CmdOnShoot();

        RaycastHit _hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask)) {
            if(_hit.collider.tag == PLAYER_TAG) {
                CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
            }

            //We hit something, call the OnHit method on server
            CmdOnHit(_hit.point, _hit.normal);
        }
    }

    [Command]
    private void CmdPlayerShot(string _playerID, int _damage) {
        Debug.Log(_playerID + " has been shot");
        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }
}
