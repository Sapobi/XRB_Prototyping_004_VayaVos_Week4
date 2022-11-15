using System;
using System.Collections.Generic;
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
	[SerializeField] private List<Button> buttonsToEnable, buttonsToDisable;

	private SongPanelManager _songPanelManager;
	private DifficultyManager _difficultyManager;

	private void Start()
	{
		_songPanelManager = FindObjectOfType<SongPanelManager>();
		_difficultyManager = FindObjectOfType<DifficultyManager>();
	}

	private void OnTriggerEnter(Collider other)
	{
		switch (type)
		{
			case ButtonType.Quit:
				QuitGame();
				break;
			case ButtonType.SongUp:
				_songPanelManager.MoveUp();
				break;
			case ButtonType.SongDown:
				_songPanelManager.MoveDown();
				break;
			case ButtonType.Start:
				SongManager.StartGame.Invoke();
				break;
			case ButtonType.Settings:
				//OpenSettings();
				break;
			case ButtonType.DifficultyDown:
				_difficultyManager.MoveDown();
				break;
			case ButtonType.DifficultyUp:
				_difficultyManager.MoveUp();
				break;
			case ButtonType.Return:
				//OpenMenu();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void OpenSettings()
	{
		ToggleButtons();
		//open settings
	}

	private void OpenMenu()
	{
		ToggleButtons();
		//open menu
	}

	private void QuitGame()
	{
		Application.Quit();
	}

	private void ToggleButtons()
	{
		foreach (var button in buttonsToEnable)
		{
			button.gameObject.SetActive(true);
		}

		foreach (var button in buttonsToDisable)
		{
			button.gameObject.SetActive(false);
		}

		gameObject.SetActive(false);
	}
}