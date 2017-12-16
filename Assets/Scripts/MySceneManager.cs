using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
	[HideInInspector]
	public static int scoreSave;
	// Use this for initialization
	void Start () {
		Cursor.visible = false;
	}

	public void ChangeScene(int id) {
		SceneManager.LoadScene(id);
	}

	public void LeaveGame () {
		Application.Quit();
	}
}
