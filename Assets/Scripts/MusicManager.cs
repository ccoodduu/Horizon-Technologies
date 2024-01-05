using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
	private int currentSong;
	[SerializeField] private AudioClip[] musicTracks;
	private AudioSource source;

	void Start()
	{
		source = GetComponent<AudioSource>();

		currentSong = -1;
	}

	void Update()
	{
		if (source.isPlaying) return;

		currentSong++;
		currentSong %= musicTracks.Length;

		source.clip = musicTracks[currentSong];
		source.Play();
	}
}
