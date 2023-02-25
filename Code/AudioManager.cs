using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	public AudioClip[] playlist;

	public AudioSource audioSource;

	private int musicIndex;

	public AudioMixerGroup soundEffectMixer;

	public static AudioManager instance;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instancede AudioManager dans la scène");
		}
		else
		{
			instance = this;
		}
	}

	private void Start()
	{
		audioSource.clip = playlist[0];
		audioSource.Play();
	}

	private void Update()
	{
		if (!audioSource.isPlaying)
		{
			PlayNextSong();
		}
	}

	private void PlayNextSong()
	{
		musicIndex = (musicIndex + 1) % playlist.Length;
		audioSource.clip = playlist[musicIndex];
		audioSource.Play();
	}

	public AudioSource PlayClipAt(AudioClip clip, Vector3 pos) 
	{
		// Play a sound at a specific position in the world
		GameObject gameObject = new GameObject("TempAudio");
		gameObject.transform.position = pos;
		AudioSource obj = gameObject.AddComponent<AudioSource>();
		obj.clip = clip;
		obj.outputAudioMixerGroup = soundEffectMixer;
		obj.Play();
		Object.Destroy(gameObject, clip.length);
		return obj;
	}
}
