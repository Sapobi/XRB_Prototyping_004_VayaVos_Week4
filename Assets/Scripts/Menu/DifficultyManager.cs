using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
	[SerializeField] private TMP_Text difficultyText;
	public List<Difficulty> possibleDifficulties;
	public Difficulty currentDifficulty;

	private int _index;

	private void Start()
	{
		SetDifficulty();
	}

	public void MoveDifficulty(int amount)
	{
		_index += amount;
		if (_index == possibleDifficulties.Count) _index = 0;
		else if (_index < 0) _index += possibleDifficulties.Count;

		SetDifficulty();
	}

	private void SetDifficulty()
	{
		if (possibleDifficulties.Count == 0) return;
		
		currentDifficulty = possibleDifficulties[_index];
		switch (currentDifficulty)
		{
			case Difficulty.Easy:
				difficultyText.text = "Easy";
				break;
			case Difficulty.Advanced:
				difficultyText.text = "Advanced";
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		SongManager.Instance.songDiff = (int)possibleDifficulties[_index];
	}

	public void SetPossibleDifficulties(List<Difficulty> difficulties)
	{
		possibleDifficulties = difficulties;

		foreach (var difficulty in possibleDifficulties)
		{
			if (currentDifficulty != difficulty) continue;
			
			_index = possibleDifficulties.IndexOf(difficulty);
			return;
		}

		_index = 0;
		SetDifficulty();
	}
}