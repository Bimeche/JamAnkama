using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
	[HideInInspector]
	public static int scoreSave;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ChangeScene(int sceneId) {
		SceneManager.LoadScene(sceneId);
	}

	public void LeaveGame () {
		Application.Quit();
	}
}
