using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {
	// X and Y size of the play area
	public static int BoardXSize, BoardYSize;
	// Current location of player and goal
	public static Vector2 playerPosition, goalPosition;
	// Flag for if the current level has been completed
	public static Vector2 playerStartPosition;
	public static bool gameWon, GameOver = false;
	// Game objects containing player and the goal
	public static GameObject player, goal = null;
	// Array containing the board information
	// p,g,o,x - player, goal, open, closed
	public char[,] board;
	// Current level
	public static int level = 1;
	// GameObjects containing the prefabs for the camera, character and goaltile
	public GameObject Camera, Character, GoalTile;
	// Array of all the game tiles
	private GameObject[] WalkableTiles, ObstacleTiles, Decor;
	// instance of the camera being used
	private GameObject theCamera;
	// Container for all the tiles
	private GameObject boardGameObject = null;
	// Flag for if the game is loading at momment
	private bool loading = true;
	// Chance to spawn decor on map
	private float DecorPercent = 0.75f;
	private Color Color_Grass;
	private float WinTimer = -1f;

	// Use this for initialization
	void Start() {
		WalkableTiles = Resources.LoadAll<GameObject>("Tiles/Walkable");
		ObstacleTiles = Resources.LoadAll<GameObject>("Tiles/Obstacle");
		Decor = Resources.LoadAll<GameObject>("Tiles/Decor");

		Color_Grass = new Color(0.271f, 0.408f, 0.4f);

		readMapFile("" + level);
		buildBoard();
	}
	
	// Update is called once per frame
	void Update() {
		if(gameWon && WinTimer > 0) {
			WinTimer -= Time.deltaTime;
			if(WinTimer < 0) { WinTimer = 0; }
		}
		if(GameOver) {
			resetCharacter();
			GameOver = false;
		}

		if(goalPosition == playerPosition) {
			if(WinTimer == -1f) {
				gameWon = true;
				WinTimer = 1f;
			} else if(WinTimer == 0) {
				WinTimer = -1f;
				levelComplete();
			}
		}

		// Try and set python variables
		while(loading) {
			loading = !setPythonData();
		}
	}

	private bool setPythonData() {
		// Send required data to the python environment
		return CodeExecutionScript.setVariables(
				board,
				BoardXSize,
				BoardYSize,
				(int) playerPosition.x,
				(int) playerPosition.y,
				(int) goalPosition.x,
				(int) goalPosition.y
			);
	}

	// Read in current map and do initialization
	private void refreshGame() {
		// Do not allow code editing on every other level
		if(level%2 == 0) {
			UIController.canEdit(false);
		} else {
			UIController.canEdit(true);
		}

		readMapFile(""+level);
		buildBoard();
		CodeExecutionScript.initChar();
		setPythonData();
	}

	private void levelComplete() {
		UIController.onGameWin();
		gameWon = false;
		clearLevel();
		level++;
		refreshGame();
	}

	private void clearLevel() {
		DestroyImmediate(theCamera);
		DestroyImmediate(boardGameObject);
		DestroyImmediate(player);
	}

	public void resetLevel() {
		clearLevel();
		if(level>1 && level%2 == 0) {
			level--;
		}

		refreshGame();
	}

	private void resetCharacter() {
		playerPosition = playerStartPosition;
		player.transform.position = new Vector3(1.5f + playerPosition.x*3, 1f, 1.5f + playerPosition.y*3);
		setPythonData();
	}
	private void buildBoard() {

		boardGameObject = new GameObject("Game Board");

		for(int z=0; z<BoardYSize; z++) {
			for(int x=0; x<BoardXSize; x++) {
				GameObject tile = null;
				switch(board[x,z]) {
						// Open Tile
						case 'o':
							tile = Instantiate(WalkableTiles[Random.Range(0, WalkableTiles.Length)], new Vector3(1.5f + x * 3, 0, 1.5f + z*3), Quaternion.identity);
							// Add some random color variation to the grass
							tile.GetComponentInChildren<Renderer>().materials[1].color = new Color(Color_Grass.r, Color_Grass.g+Random.Range(-0.02f,0.02f), Color_Grass.b);
							// Spawn some random decor
							if(Random.Range(0f,1f) > DecorPercent) {
								GameObject d = Instantiate(
									Decor[Random.Range(0, Decor.Length)],
									new Vector3(x * 3 + Random.Range(0.5f, 2f), 0.25f,z * 3 + Random.Range(0.5f, 2f)),
									Quaternion.Euler(0, Random.Range(0f, 360f), 0)
								);
								d.transform.parent = boardGameObject.transform;
							}
						break;
						// Obstacle Tile
						case 'x':
							tile = Instantiate(ObstacleTiles[Random.Range(0, ObstacleTiles.Length)], new Vector3(1.5f + x * 3, 0, 1.5f + z*3), Quaternion.Euler(0, Random.Range(0,3)*90f, 0));
							// Random color to grass
							tile.GetComponentsInChildren<Renderer>()[0].materials[1].color = new Color(Color_Grass.r, Color_Grass.g+Random.Range(-0.02f, 0.02f), Color_Grass.b);
							// Random color to leaves
							tile.GetComponentsInChildren<Renderer>()[1].materials[0].color = new Color(Color_Grass.r+Random.Range(-0.05f, 0.05f), Color_Grass.g+Random.Range(-0.05f, 0.05f), Color_Grass.b);
							// Random decor
							if(Random.Range(0f,1f) > DecorPercent-0.25f) {
								GameObject d = Instantiate(
									Decor[Random.Range(0, Decor.Length)],
									new Vector3(x * 3 + Random.Range(0.5f, 2f), 0.25f,z * 3 + Random.Range(0.5f, 2f)),
									Quaternion.Euler(0, Random.Range(0f, 360f), 0)
								);
								d.transform.parent = boardGameObject.transform;
							}			
						break;
						// Player Location
						case 'p':
							tile = Instantiate(WalkableTiles[0], new Vector3(1.5f + x * 3, 0, 1.5f + z*3), Quaternion.identity);
							// Random grass color variation
							tile.GetComponentInChildren<Renderer>().materials[1].color = new Color(Color_Grass.r, Color_Grass.g+Random.Range(-0.02f, 0.02f), Color_Grass.b);
							if(player == null) {
								player = Instantiate(Character, new Vector3(1.5f + x*3, 1f, 1.5f + z*3), Quaternion.identity);
								playerStartPosition = new Vector2(x,z);
								playerPosition = playerStartPosition;
							}
						break;
						// Goal Location
						case 'g':
							goal = Instantiate(GoalTile, new Vector3(1.5f + x * 3, 0, 1.5f + z*3), Quaternion.identity);
							goal.transform.parent = boardGameObject.transform;
							goalPosition = new Vector2(x,z);
						break;
						// Unknown
						default:
							Debug.LogError("Invalid Char On Game Board");
						break;
				}
				// If a tile was created add it to the board container
				if(tile) {
					tile.transform.parent = boardGameObject.transform;
				}
			}
		}

		// Instantiate camera at center of map
		theCamera = Instantiate(Camera, new Vector3(3f + (BoardXSize*3)/2, 20f, 4f - BoardYSize), Quaternion.identity);
		theCamera.transform.LookAt(new Vector3(3f + Mathf.Round((BoardXSize*3)/2), 0f, 1.5f + Mathf.Round((BoardYSize*3)/2)));
	}

	// Populate Board array from specified .map file
	private void readMapFile(string fileName) {
		try {
			int lineNum = -1;
			string line;
			StreamReader reader = new StreamReader("Assets/Resources/Maps/"+fileName+".map");
			using(reader) {
				do {	
					line = reader.ReadLine();
					if(line != null) {
						string[] entries = line.Split(' ');
						// Read map size from first line
						if(lineNum == -1) {
							BoardXSize = int.Parse(entries[0]);
							BoardYSize = int.Parse(entries[1]);
							board = new char[BoardXSize, BoardYSize];
						} else if(entries.Length > 0) {
							int x = 0;
							// Populate board with characters from subsequent lines
							// '(BoardYSize-1) - lineNum' allows the array to be populated in correct order
							foreach(string e in entries) {
								board[x, (BoardYSize-1) - lineNum] = char.Parse(e);
								x++;
							}
						}
						lineNum++;
					}
				} while(line != null);
				reader.Close();
			}
		} catch(System.Exception e) {
			Debug.LogError(e);
		}
	}
}
