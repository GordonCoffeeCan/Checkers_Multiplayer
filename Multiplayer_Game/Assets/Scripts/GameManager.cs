using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public MatchSettings matchSettings;

    private const string PLAYER_ID_INDEX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    private void Awake() {
        if(instance != null) {
            Debug.LogError("More than one gamemanager in the scene");
        } else {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void RegisterPlayer(string _netID, Player _player) {
        string _playerID = PLAYER_ID_INDEX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRgeisterPlayer(string _playerID) {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID) {
        return players[_playerID];
    }

    //private void OnGUI() {
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //    GUILayout.BeginVertical();

    //    foreach(string _playerID in players.Keys) {
    //        GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
    //    }
    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}
}
