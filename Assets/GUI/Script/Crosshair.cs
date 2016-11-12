using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	private Rect position;
	public Texture2D crosshairTexture;
	private bool lockedCursor = true;

	// Use this for initialization
	void Start () {
		position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height -  crosshairTexture.height) /2, crosshairTexture.width, crosshairTexture.height);
	}

	void Update() {
		if (false || Input.GetKeyDown ("escape") || lockedCursor == false) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			lockedCursor = false;
		} else {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			lockedCursor = true;
		}
		if (Input.GetKeyDown ("0")) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			lockedCursor = true;
		}
	}
	
	// Update is called once per frame
	void OnGUI () {
		GUI.DrawTexture(position, crosshairTexture);
	}
}
