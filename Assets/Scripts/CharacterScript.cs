using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour {

	Animator anim;

	Vector3 startPostion, endPosition;

	public float moveSeconds = 1f;
	private float moveTimer = 0;
	private bool isMoving = false;

	public Vector2 position;

	private Stack<string> commands;

	// Use this for initialization
	void Start () {
		startPostion = 	transform.position;
		anim = GetComponent<Animator>();
	}
	
	private void Move() {
		anim.SetBool("isMoving", true);
		isMoving = true;
		moveTimer = 0;

		startPostion = transform.position;
		}

	private void StopMove() {
		anim.SetBool("isMoving", false);
		isMoving = false;

		transform.position = endPosition;
	}

	// Update is called once per frame
	void Update () {
		if(isMoving) {
			moveTimer += Time.deltaTime;

			if(moveTimer > moveSeconds) {
				StopMove();
			} else {
				float ratio = moveTimer / moveSeconds;
				transform.position = Vector3.Lerp(startPostion, endPosition, ratio);
			}

		} else {
			if(!GameManager.gameWon) {
				if(commands!=null) {
					executeCommand();
				}
			}
		}
	}

	private void executeCommand() {
		if(commands.Count != 0) {
			string cmd = commands.Pop();
			move(cmd);
		}
	}

	public void addCommands(Stack<string> cmds) {
		this.commands = cmds;
	}


	public void move(string direction) {
		switch(direction) {
			case "up":
				if(GameManager.playerPosition.y+1 < GameManager.BoardYSize) {
					endPosition = transform.position + new Vector3(0,0,3);
					transform.LookAt(endPosition);
					Move();
					GameManager.playerPosition += new Vector2(0,1);
				}
			break;
			case "right":
				if(GameManager.playerPosition.x+1 < GameManager.BoardXSize) {
					endPosition = transform.position + new Vector3(3,0,0);
					transform.LookAt(endPosition);
					Move();
					GameManager.playerPosition += new Vector2(1,0);
				}
			break;
			case "down":
				if(GameManager.playerPosition.y-1 >= 0) {
					endPosition = transform.position + new Vector3(0,0,-3);
					transform.LookAt(endPosition);
					Move();
					GameManager.playerPosition += new Vector2(0,-1);
				}

			break;
			case "left":
				if(GameManager.playerPosition.x-1 >= 0) {
					endPosition = transform.position + new Vector3(-3,0,0);
					transform.LookAt(endPosition);
					Move();
					GameManager.playerPosition += new Vector2(-1,0);
				}

			break;
		}
	}

}