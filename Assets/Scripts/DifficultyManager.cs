using System;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
	[SerializeField] private string panel;
	public List<Difficulty> possibleDifficulties;
	public Difficulty currentDifficulty;

	private int _index;

	private void Start()
	{
		SetDifficulty();
	}

	public void MoveUp()
	{
		_index++;
		if (_index == possibleDifficulties.Count) _index = 0;

		SetDifficulty();
	}

	public void MoveDown()
	{
		_index--;
		if (_index < 0) _index += possibleDifficulties.Count;

		SetDifficulty();
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

	private void SetDifficulty()
	{
		currentDifficulty = possibleDifficulties[_index];
		switch (currentDifficulty)
		{
			case Difficulty.Easy:
				//set text easy
				break;
			case Difficulty.Advanced:
				//set text advanced
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		SongManager.Instance.songDiff = (int)possibleDifficulties[_index];
	}
}