using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowSpawnerUp : MonoBehaviour {
	public Transform cow;
	public Transform spawner;
	// Use this for initialization
	void Start () {
		Invoke("SpawnCow", 2f);
	}
	
	void SpawnCow () {
		Transform cowSpawned = Instantiate(cow, spawner.position, Quaternion.identity);
		cowSpawned.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Random.Range(300f, 700f));
		Invoke("SpawnCow", Random.Range(6f, 10f));
	}
}
