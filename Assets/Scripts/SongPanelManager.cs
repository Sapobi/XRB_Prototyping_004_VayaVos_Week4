using System.Collections.Generic;
using UnityEngine;

public class SongPanelManager : MonoBehaviour
{
	[SerializeField] private List<Song> songPanels;

	private int _index;

	private void Start()
	{
		OpenSongPanel();
	}

	public void MoveUp()
	{
		_index++;
		if (_index == songPanels.Count) _index = 0;

		OpenSongPanel();
	}

	public void MoveDown()
	{
		_index--;
		if (_index < 0) _index += songPanels.Count;

		OpenSongPanel();
	}

	private void OpenSongPanel()
	{
		foreach (var panel in songPanels)
		{
			panel.gameObject.SetActive(false);
		}

		songPanels[_index].gameObject.SetActive(true);
	}
}