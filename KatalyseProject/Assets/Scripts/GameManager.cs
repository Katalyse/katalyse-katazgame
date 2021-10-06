using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public enum Objects
	{
		porte,
		tableau,
		coffre,
		none
	};
	public Objects eObjects;
	// Singleton
	private static GameManager _instance = null;
	public static GameManager getInstance()
	{
		if (_instance == null)
		{
			_instance = new GameManager();
		}
		return _instance;
	}

	// Player reference
	public GameObject goPlayer;

	// Camera Movement
	[HideInInspector] public CameraMovement cmCameraMovement;

	// Enigme generator
	[HideInInspector] public EnigmeGenerator egEnigmeGenerator;

	// Drawing
	[HideInInspector] public Drawing dDrawing;

	// JavascriptHook reference
	public GameObject goJavascriptHook;

	//Maze Prefabs
	public Maze mazePrefab;
	private Maze mazeInstance;

	//Text Information
	[HideInInspector] public TextInformation tiTextInformation;
	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this);
		}
	}
	private void Start () {
		BeginGame();
	}
	
	private void Update () {
		/*if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}*/
	}

	private void BeginGame () {
		mazeInstance = Instantiate(mazePrefab) as Maze;
		StartCoroutine(mazeInstance.Generate());
	}

	private void RestartGame () {
		goPlayer.GetComponentInChildren<PlayerMovement>().RestartPlayer();
		StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		BeginGame();
	}

	//TO DELETE
	public void SetMovementWithInputText(string str)
	{
		StartCoroutine(LuisManager.getInstance().SubmitRequestToLuis(str));
	}

	public void TheEnd()
    {
		egEnigmeGenerator.Victory("VICTOIRE!!");
		cmCameraMovement.bisTopMapView = true;
		dDrawing.DeleteDraw();
		goPlayer.GetComponentInChildren<PlayerMovement>().SetShowWay(true);
		FindObjectOfType<Timer>().isPause = true;
	}
}