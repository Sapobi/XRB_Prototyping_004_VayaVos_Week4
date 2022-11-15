using System;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
	Easy = 1,
	Advanced = 2
}

public class Song : MonoBehaviour
{
	[SerializeField] private AudioSource songPreview, song;
	[SerializeField] private string songSystemName;
	[SerializeField] private List<Difficulty> possibleDifficulties;
	[SerializeField] private string[] songInfo;
	
	[SerializeField] private DifficultyManager _difficultyManager;
	[SerializeField] private SongPanelManager _songPanelManager;
	
	private void Start()
	{
		SongManager.StartGame.AddListener(Disable);
		Disable();
	}

	private void Disable()
	{
		enabled = false;
	}

	private void OnEnable()
	{
		for (var i = 0; i < _songPanelManager.songInfoTexts.Length; i++)
		{
			_songPanelManager.songInfoTexts[i].text = songInfo[i];
		}

		SongManager.Instance.audioSource = song;
		SongManager.Instance.songName = songSystemName;

		_difficultyManager.SetPossibleDifficulties(possibleDifficulties);

		songPreview.Play();
	}

	private void OnDisable()
	{
		songPreview.Stop();
	}
}