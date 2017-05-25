using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
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
    [HideInInspector]
    public GameObject playerUIInstance;

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer) {
            AssignRemoteLayer();
            DisableComponents();
        } else {

            //Disable player Graphics for local player
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //Create Player UI
            playerUIInstance =  Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            //Configure PlayerUI
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();

            if(ui == null) {
                Debug.LogError("No PlayerUI component on PlayerUI prefab.");
            }

            ui.SetController(GetComponent<PlayerController>());

            GetComponent<Player>().SetupPlayer();
        }
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
        Destroy(playerUIInstance);

        if (isLocalPlayer) {
            GameManager.instance.SetSceneCameraActive(true);
        }
        

        GameManager.UnRgeisterPlayer(transform.name);
    }

    private void SetLayerRecursively(GameObject obj, int newLayer) {
        obj.layer = newLayer;
        foreach(Transform child in obj.transform) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
