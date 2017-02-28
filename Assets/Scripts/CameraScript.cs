using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	
	public Vector3 centerPoint;

	private bool isSpin = false;

	// Update is called once per frame
	void Start() {
		transform.position = new Vector3(23.5f,26,6);
		transform.rotation = new Quaternion(75,0,0,98);
		/*
		float centerX = (GameManager.BoardXSize * 3) / 2f;
		float centerZ = (GameManager.BoardYSize * 3) / 2f;
		centerPoint = new Vector3(centerX, 20f, centerZ);
		transform.LookAt(centerPoint, Vector3.up);
		*/
	}

	void Update () {
		if(GameManager.gameWon) {
			isSpin = true;
		}
		if(isSpin) {
			transform.RotateAround(GameManager.goal.transform.position, Vector3.up, 40 * Time.deltaTime);
		}
		//float centerX = (GameManager.BoardXSize * 3) / 2f;
		//float centerZ = (GameManager.BoardYSize * 3) / 2f;
		//centerPoint = new Vector3(centerX, 20f, centerZ);
		//transform.LookAt(centerPoint, Vector3.up);
		/* FOLLOW PLAYER
		float centerX = GameManager.player.transform.position.x; //(GameManager.BoardXSize * 3) / 2f;
		float centerZ = GameManager.player.transform.position.z;
		float centerY = GameManager.player.transform.position.y;//(GameManager.BoardYSize * 3) / 2f;
		centerPoint = new Vector3(centerX + 5, centerY, centerZ);
		transform.LookAt(centerPoint, Vector3.up);
		*/
		//transform.RotateAround(centerPoint, Vector3.up, 20 * Time.deltaTime);
	}

	public void spin() {
		isSpin = true;
	}
}
