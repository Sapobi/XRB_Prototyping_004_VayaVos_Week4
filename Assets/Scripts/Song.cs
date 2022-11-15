using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
	Easy = 1,
	Advanced = 2
}

public class Song : MonoBehaviour
{
	[SerializeField] private AudioSource song;
	[SerializeField] private string songSystemName;
	[SerializeField] private List<Difficulty> possibleDifficulties;
	[SerializeField] private string[] panels;
	[SerializeField] private string[] panelTexts;

	[SerializeField] private DifficultyManager difficultyManager;
	
	private void Start()
	{
		enabled = false;
	}

	private void OnEnable()
	{
		for (var i = 0; i < panels.Length; i++)
		{
			panels[i] = panelTexts[i];
		}

		Debug.Log(gameObject.name);
		SongManager.Instance.audioSource = song;
		SongManager.Instance.songName = songSystemName;

		difficultyManager.SetPossibleDifficulties(possibleDifficulties);
	}
}