using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    private RectTransform thrusterFuelFill;

    private PlayerController controller;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update () {
        SetFuelAmount(controller.GetThrusterFuelAmount());
	}

    private void SetFuelAmount(float _amount) {
        thrusterFuelFill.localScale = new Vector3(1, _amount, 1);
    }

    public void SetController(PlayerController _controller) {
        controller = _controller;
    }
}
