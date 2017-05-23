using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    private Camera sceneCamera;

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer) {
            AssignRemoteLayer();
            DisableComponents();
        } else {
            sceneCamera = Camera.main;
            if (sceneCamera != null) {
                sceneCamera.gameObject.SetActive(false);
            }
        }

        RegisterPlayer();


    }

    private void RegisterPlayer() {
        string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
        transform.name = _ID;
    }

    private void AssignRemoteLayer() {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void DisableComponents() {
        for (int i = 0; i < componentsToDisable.Length; i++) {
            componentsToDisable[i].enabled = false;
        }
    }

    private void OnDisable() {
        if(sceneCamera != null) {
            sceneCamera.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
