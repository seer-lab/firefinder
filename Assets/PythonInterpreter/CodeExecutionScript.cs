using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CodeExecutionScript : MonoBehaviour {

    public InputField outputText;

    private string code;

    private static Interpreter py;

    private string output;

    private static CharacterScript charScript;

    private string pythonFunctions;

    private List<string> commands;

	// Use this for initialization
	void Start () {
        py = new Interpreter();

        initChar();
    }
    
    public static void initChar() {
        charScript = FindObjectOfType<CharacterScript>();
    }
    public static bool setVariables(int[,] b, int bX, int bY, int pX, int pY, int gX, int gY){
        if(py != null) {
            py.Scope.SetVariable("board", (object) b);
            py.Scope.SetVariable("boardX", bX);
            py.Scope.SetVariable("boardY", bY);
            py.Scope.SetVariable("playerX", pX);
            py.Scope.SetVariable("playerY", pY);
            py.Scope.SetVariable("goalX", gX);
            py.Scope.SetVariable("goalY", gY);
            // Set python variables successfully
            return true;
        }
        return false;
    }

    public void onRunClick() {
        string allCode = pythonFunctions+code;
        output = py.Compile(allCode, Microsoft.Scripting.SourceCodeKind.Statements);
        output = output.TrimEnd('\r', '\n');
        outputText.text = output;

        commands = output.Split('\n').ToList();

        Stack<string> commandStack = new Stack<string>();

        foreach(string cmd in commands) {
            commandStack.Push(cmd.TrimEnd('\r', '\n'));
        }

        charScript.addCommands(commandStack);

    }

    public void onCodeEdit(string str) {
        code = str;
    }
}
