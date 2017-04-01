using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[RequireComponent (typeof (TimeManager))]
public class GameManager : MonoBehaviour {

    public static bool gameStarted;
    public static AudioSource audioSource;

    public GameObject playerPrefab;
    public Camera cam;
    public AudioClip deathSound;
    public Text continueText, instructionalText;

    private Vector3 playerStart, cameraStart, initPlayer, initCamera;
    private Quaternion playerRot;
    private TimeManager timeManager;
    private GameObject staticWall, player;
    private bool blink;
    private int blinkTime;

	void Awake() {
        staticWall = GameObject.Find("StaticWall");
        player = GameObject.Find("Player");

        audioSource = gameObject.GetComponent<AudioSource>();

        cameraStart = cam.transform.position;
        initCamera = cameraStart;

        playerStart = player.transform.position;
        initPlayer = playerStart;
        playerRot = player.transform.rotation;

        timeManager = GetComponent<TimeManager>();
    }

	// Use this for initialization
	void Start () {

        var wallPos = cam.transform.position + new Vector3(-(640 / 2) + 7f, 0, .5f);
        staticWall.transform.position = wallPos;

        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameStarted && Time.timeScale == 0) {
			if(Input.anyKeyDown){
                timeManager.ManipulateTime(1, 1f);
				ResetGame();
			}
		}

        if (!gameStarted) {
            blinkTime++;

            if (blinkTime % 40 == 0)
            {
                blink = !blink;
            }

            continueText.canvasRenderer.SetAlpha(blink ? 0 : 1);
        }
		
	}

    void OnGoal()
    {
        cam.GetComponent<MoveCamera>().setShouldMove(false);

        Destroy(player);

        player.GetComponent<Player>().DestroyCallback -= OnPlayerKilled;
        player.GetComponent<Player>().CheckpointCallback -= OnCheckpoint;
        player.GetComponent<Player>().GoalCallback -= OnGoal;

        instructionalText.canvasRenderer.SetAlpha(1);
        instructionalText.color = Color.green;
        instructionalText.text = "You're totally rad!";

        cameraStart = initCamera;
        playerStart = initPlayer;

        timeManager.ManipulateTime(0, 5.5f);
        gameStarted = false;
    }

    void OnPlayerKilled() {

        audioSource.PlayOneShot(deathSound);
        cam.GetComponent<MoveCamera>().setShouldMove(false);

        player.GetComponent<Player>().DestroyCallback -= OnPlayerKilled;
        player.GetComponent<Player>().CheckpointCallback -= OnCheckpoint;
        player.GetComponent<Player>().GoalCallback -= OnGoal;

        continueText.text = "Press any key to restart";

        timeManager.ManipulateTime(0, 5.5f);
        gameStarted = false;
    }

    void OnCheckpoint()
    {
        cameraStart = cam.transform.position;
        playerStart = player.transform.position;
    }

	void ResetGame(){
        cam.transform.Translate(cameraStart.x - cam.transform.position.x, cameraStart.y - cam.transform.position.y, 0);
        cam.GetComponent<MoveCamera>().setShouldMove(true);

        if(player == null)
            player = Instantiate(playerPrefab, playerStart, playerRot) as GameObject;

        var playerDestroyScript = player.GetComponent<Player>();
        playerDestroyScript.DestroyCallback += OnPlayerKilled;
        playerDestroyScript.CheckpointCallback += OnCheckpoint;
        playerDestroyScript.GoalCallback += OnGoal;

        continueText.canvasRenderer.SetAlpha(0);
        instructionalText.canvasRenderer.SetAlpha(0);

        gameStarted = true;

	}


}
