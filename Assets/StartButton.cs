using UnityEngine;

public class StartButton : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		SongManager.StartGame.Invoke();
	}
}