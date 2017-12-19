using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
	[HideInInspector]
	public static int scoreSave;

	private void Start () {
		Cursor.visible = true;
	}

	public void ChangeScene(int id) {
		SceneManager.LoadScene(id);
	}

	public void LeaveGame () {
		Application.Quit();
	}
}
