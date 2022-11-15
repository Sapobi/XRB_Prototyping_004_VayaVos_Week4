using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
	Easy = 0,
	Advanced = 1
}

public class Song : MonoBehaviour
{
	[SerializeField] private AudioSource song;
	[SerializeField] private string songSystemName;
	[SerializeField] private List<Difficulty> possibleDifficulties;
	[SerializeField] private string[] panels;
	[SerializeField] private string[] panelTexts;

	private DifficultyManager _difficultyManager;
	
	private void Start()
	{
		_difficultyManager = FindObjectOfType<DifficultyManager>();
	}

	private void OnEnable()
	{
		for (var i = 0; i < panels.Length; i++)
		{
			panels[i] = panelTexts[i];
		}

		SongManager.Instance.audioSource = song;
		SongManager.Instance.songName = songSystemName;

		_difficultyManager.SetPossibleDifficulties(possibleDifficulties);
	}
}