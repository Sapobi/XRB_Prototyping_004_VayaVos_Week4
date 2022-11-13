using UnityEngine;

public class Note : MonoBehaviour
{
	double timeInstantiated;
	public float assignedTime;
	public Vector3 spawnPos, tapPos;
	void Start()
	{
		timeInstantiated = SongManager.GetAudioSourceTime();
	}

	// Update is called once per frame
	void Update()
	{
		double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
		float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime));

        
		if (t > 1)
		{
			Destroy(gameObject);
		}
		else
		{
			transform.localPosition = Vector3.Lerp(spawnPos, tapPos, t); 
			GetComponentInChildren<SpriteRenderer>().enabled = true;
		}
	}
}