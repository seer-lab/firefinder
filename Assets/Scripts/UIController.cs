using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public GameObject manPage;
	public static GameObject editorPage;

	public GameObject GM;

	public static GameObject winScreen;

	void Awake() {
		winScreen = GameObject.Find("WinScreen");	
		if(winScreen) { winScreen.SetActive(false); }

		editorPage = GameObject.Find("CodeEditor");	
		if(editorPage) { editorPage.SetActive(false); }

		InputField t = manPage.GetComponent<InputField>();
		t.text = "Variables:\nplayerX, playerY\ngoalX, goalY\nboard(2d array)\n\nDirections:\n'up', 'right', 'down', 'left'\n\nFunctions:\nmove(direction) # Attempts to move\ncheck(direction) # Returns if you can move\n";
	}

	public static void onGameWin() {
		winScreen.SetActive(true);
	}

	public void onManClick() {
		editorPage.SetActive(false);
		manPage.SetActive(true);
	}

	public void onEditorClick() {
		manPage.SetActive(false);
		editorPage.SetActive(true);
	}

	public void onNextLevelClick() {
		winScreen.SetActive(false);
	}

	public void onResetClick() {
		GM.GetComponent<GameManager>().resetLevel();
	}

	public static void canEdit(bool edit) {
		editorPage.GetComponent<InputField>().readOnly =  !edit;
	}


}
