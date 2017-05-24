using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDraw";

    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    private GameObject playerUIPrefab;
    private GameObject playerUIInstance;

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

            //Disable player Graphics for local player
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //Create Player UI
            playerUIInstance =  Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;
        }

        GetComponent<Player>().Setup();
    }

    // Update is called once per frame
    void Update() {

    }

    public override void OnStartClient() {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, _player);
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

        Destroy(playerUIInstance);

        GameManager.UnRgeisterPlayer(transform.name);
    }

    private void SetLayerRecursively(GameObject obj, int newLayer) {
        obj.layer = newLayer;
        foreach(Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
