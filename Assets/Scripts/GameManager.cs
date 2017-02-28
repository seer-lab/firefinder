using UnityEngine;

public class GameManager : MonoBehaviour {

	public static int BoardXSize = 10;
	public static int BoardYSize = 10;

	public GameObject Camera;
	public GameObject Character;

	public GameObject GoalTile;

	private GameObject[] Tiles;  

	public static GameObject player;

	public int[,] board;
	public static Vector2 playerPosition;

	public static Vector2 goalPosition;

	public static bool gameWon = false;

	public static GameObject goal = null;

	private bool loading = true;

	// Use this for initialization
	void Start() {
		 Tiles = Resources.LoadAll<GameObject>("Tiles");
		 if(Tiles.Length>0) {
		 	GenerateBoard();
		 }
	}
	
	// Update is called once per frame
	void Update() {
		if(goalPosition == playerPosition) {
			gameWon = true;
		}
		while(loading) {
			loading = !CodeExecutionScript.setVariables(
				board,
				(int)playerPosition.x,
				(int)playerPosition.y,
				(int)goalPosition.x,
				(int)goalPosition.y
			);
		}
	}

	private void GenerateBoard() {

		GameObject boardGameObject = new GameObject("Game Board");

		board = new int[BoardXSize, BoardYSize];

		for(int z=0; z<BoardYSize; z++) {
			for(int x=0; x<BoardXSize; x++) {
				int rand = Random.Range(0, Tiles.Length-1);
				board[x,z] = rand;
				if(rand == 0 && player == null && x>1 && z>1) {
					player = Instantiate(Character, new Vector3(1.5f + x*3, 1f, 1.5f + z*3), Quaternion.identity);
					playerPosition = new Vector2(x,z);
				}
				if(rand == 0 && goal == null && player && x>5 && z>5) {
					goal = Instantiate(GoalTile, new Vector3(3f + x * 3, 0, 3f + z*3), Quaternion.identity);
					goal.transform.parent = boardGameObject.transform;
					goalPosition = new Vector2(x,z);
				}
				GameObject tile = Instantiate(Tiles[rand], new Vector3(3f + x * 3, 0, 3f + z*3), Quaternion.identity);
				tile.transform.parent = boardGameObject.transform;
			}
		}

		Instantiate(Camera, new Vector3(player.transform.position.x -9f, 10f,player.transform.position.z - 9f),Quaternion.identity);
	}

}
