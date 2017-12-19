using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public Transform cow;
	public RectTransform scoreSheet;
	public RectTransform pausePanel;
	public RectTransform pauseButtonPanel;
	public RectTransform currentScorePanel;
	public float spawnForce = 100f;
	public Text scoreText;
	public Text bestScore;
	public Text finalScore;
	public Text scoreToBeat;
	private int playerScore = 0;
	private float scoreUpdate = 0.2f;
	private int numberOfCowsSpawned = 0;
	public float spawnTime = 2f;
	private GameObject[] spawns;
	private List<Transform> cowsSpawned;
	private bool paused = false;
	int palier = 5;
	private bool gameEnded;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1f;
		gameEnded = false;
		SoundManager.instance.PauseMusic(false);
		cowsSpawned = new List<Transform>();
		spawns = GameObject.FindGameObjectsWithTag("CowSpawn");
		Invoke("SpawnCow", spawnTime);
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
		bool cowSpawned = false;
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
				spawnForce = Random.Range(50f, 200f);
				Vector2 angle;
				if (spawnPoint.transform.position.x < 0)
					angle = new Vector2(Random.Range(-spawnPoint.transform.position.x / 3, -spawnPoint.transform.position.x), -Random.Range(3f, 7f));
				else if (spawnPoint.transform.position.x > 0)
					angle = new Vector2(Random.Range(-spawnPoint.transform.position.x, -spawnPoint.transform.position.x / 3), -Random.Range(3f, 7f));
				else
					angle = new Vector2(Random.Range(-6f, 6f), -Random.Range(3f, 7f));
				temp.GetComponent<Rigidbody2D>().AddForce(angle * spawnForce);
				numberOfCowsSpawned++;
				cowSpawned = true;
			}
		}

		// si aucune vache n'a spawn on en spawn une sur un spawner choisit au hasard
		if (!cowSpawned)
		{
			GameObject spawnPoint = spawns[Random.Range(0,7)];
			Transform temp = Instantiate(cow, spawnPoint.transform.position, Quaternion.identity);
			cowsSpawned.Add(temp);
			spawnForce = Random.Range(50f, 200f);
			Vector2 angle;
			if (spawnPoint.transform.position.x < 0)
				angle = new Vector2(Random.Range(-spawnPoint.transform.position.x / 3, -spawnPoint.transform.position.x), -Random.Range(3f, 7f));
			else if (spawnPoint.transform.position.x > 0)
				angle = new Vector2(Random.Range(-spawnPoint.transform.position.x, -spawnPoint.transform.position.x / 3), -Random.Range(3f, 7f));
			else
				angle = new Vector2(Random.Range(-6f, 6f), -Random.Range(3f, 7f));
			temp.GetComponent<Rigidbody2D>().AddForce(angle * spawnForce);
			numberOfCowsSpawned++;
		}

		if (numberOfCowsSpawned >= 15 && palier >= 0) {
			palier = palier - 1;
			numberOfCowsSpawned = 0;
		}
		Invoke("SpawnCow", spawnTime);
	}

	public void DestroyCow (GameObject go) {
		cowsSpawned.Remove(go.transform);
		playerScore += 10;
		Destroy(go);
		Debug.Log("Cow destroyed");
	}

	public void PauseGame () {
		if (gameEnded)
			return;
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

	public void EndGame(GameObject go) {
		Time.timeScale = 0;
		gameEnded = true;
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

		pauseButtonPanel.GetComponent<CanvasGroup>().alpha = 0;
		pauseButtonPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

		currentScorePanel.GetComponent<CanvasGroup>().alpha = 0;
		currentScorePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

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