using UnityEngine;

public class Note : MonoBehaviour
{
	double timeInstantiated;
	public float assignedTime;
	public Vector3 spawnPos, tapPos, despawnPos;

	void Start()
	{
		timeInstantiated = SongManager.GetAudioSourceTime();
	}

	// Update is called once per frame
	void Update()
	{
		double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
		float t = (float)(timeSinceInstantiated / SongManager.Instance.noteTime);


		if (t <= 1)
		{
			if (t <= 0.15f)
			{
				transform.localScale = Vector3.Lerp(Vector3.zero,Vector3.one * 0.05f,  t / 0.15f);
			}
			transform.localPosition = Vector3.Lerp(spawnPos, tapPos, t);
		}
		else if (t < 1.3f)
		{
			transform.localPosition = Vector3.Lerp(tapPos, despawnPos, (t - 1) / 0.3f);
			transform.localScale = Vector3.Lerp(Vector3.one * 0.05f, Vector3.zero, (t - 1) / 0.3f);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}