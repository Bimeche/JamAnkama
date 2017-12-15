using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public Transform cow;
	public float spawnForce = 100f;
	private int numberOfCowsSpawned = 0;
	public float spawnTime = 2f;
	private GameObject[] spawns;
	private List<Transform> cowsSpawned;

	// Use this for initialization
	void Start () {
		cowsSpawned = new List<Transform>();
		spawns = GameObject.FindGameObjectsWithTag("CowSpawn");
		Invoke("SpawnCow", spawnTime);
	}

	// Update is called once per frame
	void FixedUpdate () {
	}

	/*private void SpawnCow () {
		//Debug.Log("spawned cow");
		GameObject spawnPoint = spawns[Random.Range(0, spawns.Length)];
		Transform temp = Instantiate(cow, spawnPoint.transform.position, Quaternion.identity);
		cowsSpawned.Add(temp);
		temp.GetComponent<Rigidbody2D>().AddForce(-spawnPoint.transform.position * spawnForce);
		numberOfCowsSpawned++;
		spawnTime = baseSpawnTime - numberOfCowsSpawned * spawnAcceleration;
		if (spawnTime < 0.5f)
			spawnTime = 0.5f;
		Invoke("SpawnCow", spawnTime);
	}*/

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
		Destroy(go);
		Debug.Log("Cow destroyed");
	}

	private void EndGame(GameObject go) {
		Debug.Log("Go to end menu");
		Destroy(go);
	}


}