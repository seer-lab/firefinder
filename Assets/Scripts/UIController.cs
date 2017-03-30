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
		TextAsset docText = Resources.Load("Text/Documentation") as TextAsset;
		t.text = docText.text;
	}

	public static void onGameWin() {
		winScreen.SetActive(true);
		if(GameManager.level % 2 == 1) {
			winScreen.GetComponentsInChildren<Text>()[1].text = "Next Level is a challenge level\nYou need to use the same code from the last level\nIf it doesnt work try Restarting";
		} else {
			winScreen.GetComponentsInChildren<Text>()[1].text = "Challenge Complete, Lets try a harder map!";
		}
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
		editorPage.GetComponent<InputField>().readOnly = !edit;
	}


}
