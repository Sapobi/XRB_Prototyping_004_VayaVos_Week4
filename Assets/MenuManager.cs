using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	[SerializeField] private DifficultyManager difficultyManager;
	[SerializeField] private SongPanelManager songPanelManager;
	[SerializeField] private List<Button> allButtons, returnButton, menuButtons;
	[SerializeField] private List<GameObject> screens;

	private void Start()
	{
		SongManager.EndGame.AddListener(OpenScore);
		OpenMenu();
	}

	public void StartGame()
	{
		ToggleButtons(new List<Button>(), allButtons);
		OpenScreen(2);

		SongManager.StartGame.Invoke();
	}

	public void MoveSong(int amount)
	{
		songPanelManager.MoveSong(amount);
	}

	public void MoveDifficulty(int amount)
	{
		difficultyManager.MoveDifficulty(amount);
	}

	public void OpenMenu()
	{
		ToggleButtons(menuButtons, allButtons.Except(menuButtons).ToList());
		OpenScreen(0);
	}

	private void OpenScore()
	{
		ToggleButtons(returnButton, allButtons.Except(returnButton).ToList());
		OpenScreen(3);
	}

	public void OpenSettings()
	{
		ToggleButtons(allButtons.Except(menuButtons).ToList(), menuButtons);
		OpenScreen(1);
	}

	private void ToggleButtons(List<Button> buttonsToEnable, List<Button> buttonsToDisable)
	{
		foreach (var button in buttonsToEnable)
		{
			button.gameObject.SetActive(true);
		}

		foreach (var button in buttonsToDisable)
		{
			button.gameObject.SetActive(false);
		}
	}

	private void OpenScreen(int i)
	{
		foreach (var screen in screens)
		{
			screen.SetActive(false);
		}
		screens[i].SetActive(true);
	}
}