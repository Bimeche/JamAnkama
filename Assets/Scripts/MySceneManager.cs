using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SceneManager.sceneLoaded += ResizeCameraRatio;
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ChangeScene(int sceneId) {
		SceneManager.LoadScene(sceneId);
	}

	void ResizeCameraRatio (Scene scene, LoadSceneMode mode) {
		GetComponent<CameraZoomController>().ResizeRatio();
	}
}
