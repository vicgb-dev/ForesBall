using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
	public List<AudioClip> audioClips = new List<AudioClip>();

	public static SoundManager Instance;

	private void Awake()
	{
		Instance = this;
	}

	public void PlayAudio(string songname)
	{
		AudioClip newAudio = audioClips.Where(audio => audio.name.Equals(songname)).First();

		if (newAudio == null) return;

		GameObject audioSourceGO = new GameObject();
		audioSourceGO.transform.SetParent(gameObject.transform);
		AudioSource audioSource = audioSourceGO.AddComponent<AudioSource>();
		audioSource.clip = newAudio;
		audioSource.Play();
	}
}
