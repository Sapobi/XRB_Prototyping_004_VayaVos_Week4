using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance;
	public AudioSource hitSFX;
	public AudioSource missSFX;
	public TextMeshPro scoreText;
	static int comboScore;

	void Start()
	{
		Instance = this;
		SongManager.StartGame.AddListener(ResetScore);
	}

	private static void ResetScore()
	{
		comboScore = 0;
	}

	public static void Hit()
	{
		comboScore += 1;
		Instance.hitSFX.Play();
	}

	public static void Miss()
	{
		ResetScore();
		Instance.missSFX.Play();
	}

	private void Update()
	{
		scoreText.text = comboScore.ToString();
	}
}