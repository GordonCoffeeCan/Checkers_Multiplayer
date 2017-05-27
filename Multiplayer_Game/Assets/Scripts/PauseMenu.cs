using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : MonoBehaviour {

    public static bool IsOn = false;

    private NetworkManager networkManager;

	// Use this for initialization
	void Start () {
        networkManager = NetworkManager.singleton;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LeaveRoom() {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
