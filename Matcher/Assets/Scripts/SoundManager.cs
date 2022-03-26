using UnityEngine;


public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	private AudioSource sfx;

	// Use this for initialization
	void Start()
	{
		instance = GetComponent<SoundManager>();
		sfx = GetComponent<AudioSource>();
	}

	public void Play()
	{
		sfx.Play();
	}
}

