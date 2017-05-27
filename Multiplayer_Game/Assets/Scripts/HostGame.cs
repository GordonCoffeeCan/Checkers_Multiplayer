using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 4;

    private string roomName;

    private NetworkManager networkMangager;

	// Use this for initialization
	void Start () {
        networkMangager = NetworkManager.singleton;
        if(networkMangager.matchMaker == null) {
            networkMangager.StartMatchMaker();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetRoomName(string _name) {
        roomName = _name;
    }

    public void CreateRoom() {
        if(roomName != "" && roomName != null) {
            Debug.Log(roomName + ", size " + roomSize);

            //Create room
            networkMangager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkMangager.OnMatchCreate);
            
        }
    }
}
