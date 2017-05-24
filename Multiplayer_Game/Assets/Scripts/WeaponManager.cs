using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;

    

	// Use this for initialization
	void Start () {
        EquipWeapon(primaryWeapon);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void EquipWeapon(PlayerWeapon _weapon) {
        currentWeapon = _weapon;

        GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        if (isLocalPlayer) {
            _weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
        }
    }

    public PlayerWeapon GetCurrentWeapon() {
        return currentWeapon;
    }


}
