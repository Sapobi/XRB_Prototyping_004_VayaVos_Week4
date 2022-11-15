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

	public void MoveSong(int amount)
	{
		_index += amount;
		if (_index == songPanels.Count) _index = 0;
		else if (_index < 0) _index += songPanels.Count;

		OpenSongPanel();
	}

	private void OpenSongPanel()
	{
		foreach (var panel in songPanels)
		{
			panel.enabled = false;
		}

		songPanels[_index].enabled = true;
	}
}