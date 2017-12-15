using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void OnEnable () {
		CowManager.OnCowDied += DestroyCow;
	}

	private void OnDisable () {
		CowManager.OnCowDied -= DestroyCow;
	}

	private void DestroyCow (GameObject go) {
		Destroy(go);
		Debug.Log("Cow destroyed");
	}
}
