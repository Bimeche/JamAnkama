using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(AudioSource))]

public class PlayIntro : MonoBehaviour {
	public RawImage image;
	public RectTransform skipIntro;

	public VideoClip videoToPlay;

	private VideoPlayer videoPlayer;
	private VideoSource videoSource;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		Application.runInBackground = true;
		SoundManager.instance.PauseMusic(true);
		skipIntro.GetComponent<CanvasGroup>().alpha = 0;
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
		
		image.texture = videoPlayer.texture;
		videoPlayer.Play();
		
		while (videoPlayer.isPlaying)
		{
			yield return null;
		}
		
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown)
		{
			if (skipIntro.GetComponent<CanvasGroup>().alpha == 0)
				skipIntro.GetComponent<CanvasGroup>().alpha = 1;
			else
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}
