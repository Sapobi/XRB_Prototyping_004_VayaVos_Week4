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
	
	[SerializeField] private DifficultyManager difficultyManager;
	[SerializeField] private SongPanelManager songPanelManager;
	
	private void Start()
	{
		SongManager.StartGame.AddListener(Disable);
	}

	private void Disable()
	{
		enabled = false;
	}

	private void OnEnable()
	{
		for (var i = 0; i < songPanelManager.songInfoTexts.Length; i++)
		{
			songPanelManager.songInfoTexts[i].text = songInfo[i];
		}

		SongManager.Instance.audioSource = song;
		SongManager.Instance.songName = songSystemName;

		difficultyManager.SetPossibleDifficulties(possibleDifficulties);

		songPreview.Play();
	}

	private void OnDisable()
	{
		songPreview.Stop();
	}
}