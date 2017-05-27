using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    private RectTransform thrusterFuelFill;

    [SerializeField]
    private GameObject pauseMenu;

    private PlayerController controller;

	// Use this for initialization
	void Start () {
        PauseMenu.IsOn = false;
	}

    // Update is called once per frame
    void Update () {
        SetFuelAmount(controller.GetThrusterFuelAmount());

        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePauseMenu();
        }
	}

    private void SetFuelAmount(float _amount) {
        thrusterFuelFill.localScale = new Vector3(1, _amount, 1);
    }

    public void SetController(PlayerController _controller) {
        controller = _controller;
    }

    private void TogglePauseMenu() {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;
    }
}
