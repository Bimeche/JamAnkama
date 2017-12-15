using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public Transform cow;
	public float baseSpawnTime = 5f;
	public float spawnAcceleration = 0.2f;
	public float spawnForce = 100f;
	private int numberOfCowsSpawned = 0;
	private float spawnTime;
	private GameObject[] spawns;
	private List<Transform> cowsSpawned;

	// Use this for initialization
	void Start () {
		cowsSpawned = new List<Transform>();
		spawns = GameObject.FindGameObjectsWithTag("CowSpawn");
		spawnTime = baseSpawnTime;
		Invoke("SpawnCow", spawnTime);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}

	private void SpawnCow () {
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
