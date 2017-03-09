using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject uiController;

	public static int BoardXSize = 6;
	public static int BoardYSize = 6;

	public GameObject Camera;
	public GameObject Character;

	public GameObject GoalTile;

	private GameObject[] Tiles;

	private GameObject theCamera;

	public static GameObject player;

	public int[,] board;
	public static Vector2 playerPosition;

	public static Vector2 goalPosition;

	public static bool gameWon = false;

	public static GameObject goal = null;

	private GameObject boardGameObject = null;

	private bool loading = true;

	public int level = 0;

	// Use this for initialization
	void Start() {
		 Tiles = Resources.LoadAll<GameObject>("Tiles");
		 if(Tiles.Length>0) {
			 GenerateBoard();
			 //readMapFile("01");
		 }
	}
	
	// Update is called once per frame
	void Update() {
		if(goalPosition == playerPosition) {
			gameWon = true;
			if(gameWon) {
				levelComplete();
			}
		}
		while(loading) {
			loading = !setPythonData();
		}
	}

	private bool setPythonData() {
		return CodeExecutionScript.setVariables(
				board,
				BoardXSize,
				BoardYSize,
				(int)playerPosition.x,
				(int)playerPosition.y,
				(int)goalPosition.x,
				(int)goalPosition.y
			);
	}

	private void levelComplete() {
		UIController.onGameWin();
		gameWon = false;
		clearLevel();
		level++;

		if(level%2 == 1) {
			UIController.canEdit(false);
			BoardXSize = 10;
			BoardYSize = 10;
		} else {
			UIController.canEdit(true);
			BoardXSize = 6;
			BoardYSize = 6;
		}

		GenerateBoard();
		CodeExecutionScript.initChar();
		setPythonData();
	}

	private void clearLevel() {
		DestroyImmediate(theCamera);
		DestroyImmediate(boardGameObject);
		DestroyImmediate(player);
	}

	public void resetLevel() {
		clearLevel();
		if(level>0 && level%2 == 1) {
			level--;

			if(level%2 == 1) {
				UIController.canEdit(false);
				BoardXSize = 10;
				BoardYSize = 10;
			} else {
				UIController.canEdit(true);
				BoardXSize = 6;
				BoardYSize = 6;
			}
		}

		GenerateBoard();
		CodeExecutionScript.initChar();
		setPythonData();
	}

	private void GenerateBoard() {

		boardGameObject = new GameObject("Game Board");

		board = new int[BoardXSize, BoardYSize];

		for(int z=0; z<BoardYSize; z++) {
			for(int x=0; x<BoardXSize; x++) {
				int rand = Random.Range(-1, 1+((level/2)-level%2));
				board[x,z] = rand;
				if(rand == -1 && player == null && x > 1) {
					player = Instantiate(Character, new Vector3(1.5f + x*3, 1f, 1.5f + z*3), Quaternion.identity);
					playerPosition = new Vector2(x,z);
				}
				GameObject tile = null;
				if(goal == null && player && z > BoardYSize/2 && x > BoardXSize/2 && rand == -1) {
					goal = Instantiate(GoalTile, new Vector3(3f + x * 3, 0, 3f + z*3), Quaternion.identity);
					goal.transform.parent = boardGameObject.transform;
					goalPosition = new Vector2(x,z);
				} else {
					if(rand == 0 || rand == -1) {
						board[x,z] = 0;
						tile = Instantiate(Tiles[Random.Range(0,3)], new Vector3(3f + x * 3, 0, 3f + z*3), Quaternion.identity);
					} else if (rand == 1) {
						tile = Instantiate(Tiles[Random.Range(3,5)], new Vector3(3f + x * 3, 0, 3f + z*3), Quaternion.identity);					
					}
					tile.transform.parent = boardGameObject.transform;
				}
			}
		}
		theCamera = Instantiate(Camera, new Vector3(player.transform.position.x - 9f, 10f, player.transform.position.z - 9f), Quaternion.identity);
	}

	private void populateBoard() {

		boardGameObject = new GameObject("Game Board");

		for(int z=0; z<BoardYSize; z++) {
			for(int x=0; x<BoardXSize; x++) {
				if(board[x,z] == -2 && player == null) {
					player = Instantiate(Character, new Vector3(1.5f + x*3, 1f, 1.5f + z*3), Quaternion.identity);
					playerPosition = new Vector2(x,z);
				}
				GameObject tile = null;
				if(board[x,z] == -1) {
					goal = Instantiate(GoalTile, new Vector3(3f + x * 3, 0, 3f + z*3), Quaternion.identity);
					goal.transform.parent = boardGameObject.transform;
					goalPosition = new Vector2(x,z);
				} else {
					if(board[x,z] == 0) {
						tile = Instantiate(Tiles[Random.Range(0,3)], new Vector3(3f + x * 3, 0, 3f + z*3), Quaternion.identity);
					} else if (board[x,z] == 1) {
						tile = Instantiate(Tiles[Random.Range(3,5)], new Vector3(3f + x * 3, 0, 3f + z*3), Quaternion.identity);					
					}
					tile.transform.parent = boardGameObject.transform;
				}
			}
		}
		theCamera = Instantiate(Camera, new Vector3(player.transform.position.x - 9f, 10f, player.transform.position.z - 9f), Quaternion.identity);
	}

	private void readMapFile(string level) {
		try {
			int lineNum = 0;
			string line;
			StreamReader reader = new StreamReader("Assets/Resources/Maps/level"+level+".map");
			using(reader) {
				do {	
					line = reader.ReadLine();
					if(line != null) {
						string[] entries = line.Split(' ');
						if(lineNum == 0) {
							BoardXSize = int.Parse(entries[0]);
							BoardYSize = int.Parse(entries[1]);
							board = new int[BoardXSize, BoardYSize];
						} else if(entries.Length > 0) {
							int x = 0;
							foreach(string e in entries) {
								board[x, lineNum] = int.Parse(e);
								x++;
							}
							lineNum++;
						}
					}
				} while(line != null);
				reader.Close();

				populateBoard();
			}
		} catch(System.Exception e) {
			Debug.LogError(e.Message);
		}
	}
}
