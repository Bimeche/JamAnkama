using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(AudioSource))]

public class PlayIntro : MonoBehaviour {
	public RawImage image;

	public VideoClip videoToPlay;

	private VideoPlayer videoPlayer;
	private VideoSource videoSource;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		SoundManager.instance.PauseMusic(true);
		StartCoroutine(playVideo());
	}

	IEnumerator playVideo () {
		videoPlayer = gameObject.AddComponent<VideoPlayer>();
		
		videoPlayer.playOnAwake = false;

		videoPlayer.source = VideoSource.VideoClip;
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		videoPlayer.EnableAudioTrack(0, true);
		
		videoPlayer.clip = videoToPlay;
		videoPlayer.Prepare();

		while (!videoPlayer.isPrepared)
		{
			yield return null;
		}

		Debug.Log("Done Preparing Video");
		
		image.texture = videoPlayer.texture;
		videoPlayer.Play();

		Debug.Log("Playing Video");
		while (videoPlayer.isPlaying)
		{
			Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
			yield return null;
		}

		Debug.Log("Done Playing Video");
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	// Update is called once per frame
	void Update () {

	}
}
