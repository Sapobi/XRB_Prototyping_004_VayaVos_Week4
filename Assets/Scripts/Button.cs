using System;
using UnityEngine;

public enum ButtonType
{
	Quit = 1,
	SongUp = 2,
	SongDown = 3,
	Start = 4,
	Settings = 5,
	DifficultyDown = 6,
	DifficultyUp = 7,
	Return = 8
}

public class Button : MonoBehaviour
{
	[SerializeField] private ButtonType type;
	[SerializeField] private MenuManager menuManager;

	private void OnTriggerEnter(Collider other)
	{
		switch (type)
		{
			case ButtonType.Quit:
				QuitGame();
				break;
			case ButtonType.SongUp:
				menuManager.MoveSong(1);
				break;
			case ButtonType.SongDown:
				menuManager.MoveSong(-1);
				break;
			case ButtonType.Start:
				menuManager.StartGame();
				break;
			case ButtonType.Settings:
				menuManager.OpenSettings();
				break;
			case ButtonType.DifficultyDown:
				menuManager.MoveDifficulty(-1);
				break;
			case ButtonType.DifficultyUp:
				menuManager.MoveDifficulty(1);
				break;
			case ButtonType.Return:
				menuManager.OpenMenu();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void QuitGame()
	{
		Application.Quit();
	}
}