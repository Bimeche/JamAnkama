using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public Transform cow;
	public RectTransform scoreSheet;
	public RectTransform pausePanel;
	public RectTransform pauseButtonPanel;
	public float spawnForce = 100f;
	public Text scoreText;
	public Text bestScore;
	public Text finalScore;
	public Text scoreToBeat;
	private int playerScore = 0;
	private float scoreUpdate = 0.2f;
	private int highScore = 0;
	private int lastHighScore = 0;
	private int numberOfCowsSpawned = 0;
	public float spawnTime = 2f;
	private GameObject[] spawns;
	private List<Transform> cowsSpawned;
	private bool paused = false;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1f;
		cowsSpawned = new List<Transform>();
		spawns = GameObject.FindGameObjectsWithTag("CowSpawn");
		Invoke("SpawnCow", spawnTime);
		pausePanel.GetComponent<CanvasGroup>().alpha = 0;
		pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(scoreUpdate < 0)
		{
			scoreUpdate = 0.2f;
			playerScore += 1;
		}
		scoreUpdate -= Time.fixedDeltaTime;
		scoreText.text = "Score : " + playerScore;
	}

	private void SpawnCow () {
		int palier = 5;
		List<int> intList = new List<int>();
		for (int h = 0; h < 7; h++) {
			intList.Add (h);
		}
		for (int i = 0; i < palier; i++) {//pallier part à 5
			intList.RemoveAt (Random.Range (0, (intList.Count)));
		}
		foreach (int j in intList) {
			int rand = Random.Range (0, 2);
			if (rand == 1) {
				GameObject spawnPoint = spawns [j];
				Transform temp = Instantiate(cow, spawnPoint.transform.position, Quaternion.identity);
				cowsSpawned.Add(temp);
				temp.GetComponent<Rigidbody2D>().AddForce(-spawnPoint.transform.position * spawnForce);
				numberOfCowsSpawned++;
			}
		}
		if (numberOfCowsSpawned >= 15) {
			palier = palier - 1;
			numberOfCowsSpawned = 0;
			if (palier < 0) {
				palier = 0;
			}
		}
		Invoke("SpawnCow", spawnTime);
	}

	private void OnEnable () {
		CowManager.OnCowDied += DestroyCow;
		WolfManager.OnWolfDied += EndGame;
	}

	private void OnDisable () {
		CowManager.OnCowDied -= DestroyCow;
		WolfManager.OnWolfDied -= EndGame;
	}

	private void DestroyCow (GameObject go) {
		cowsSpawned.Remove(go.transform);
		playerScore += 10;
		Destroy(go);
		Debug.Log("Cow destroyed");
	}

	public void PauseGame () {
		if (!paused)
		{
			paused = true;
			Time.timeScale = 0f;
			pausePanel.GetComponent<CanvasGroup>().alpha = 1;
			pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else
		{
			paused = false;
			Time.timeScale = 1f;
			pausePanel.GetComponent<CanvasGroup>().alpha = 0;
			pausePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}

	}

	private void EndGame(GameObject go) {
		Time.timeScale = 0;
		Destroy(go);
		Transform toDestroy;
		for(int i = cowsSpawned.Count - 1; i >= 0; i--)
		{
			toDestroy = cowsSpawned[i];
			cowsSpawned.RemoveAt(i);
			Destroy(toDestroy.gameObject);
		}
		scoreSheet.GetComponent<CanvasGroup>().alpha = 1;
		scoreSheet.GetComponent<CanvasGroup>().blocksRaycasts = true;
		finalScore.text = "" + playerScore;
		if (playerScore > MySceneManager.scoreSave)
		{
			MySceneManager.scoreSave = playerScore;
			bestScore.CrossFadeAlpha(1f, 0f, true);
			scoreToBeat.CrossFadeAlpha(0f, 0f, true);
		}
		else
		{
			scoreToBeat.text = "Highscore : " + MySceneManager.scoreSave;
			bestScore.CrossFadeAlpha(0f, 0f, true);
			scoreToBeat.CrossFadeAlpha(1f, 0f, true);
		}
	}
}