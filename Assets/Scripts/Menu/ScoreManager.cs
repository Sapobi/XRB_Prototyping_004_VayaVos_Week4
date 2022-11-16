using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	[SerializeField] private AudioSource hitSFX;
	[SerializeField] private AudioSource missSFX;
	[SerializeField] private TMP_Text comboText, maxComboText, hitRatioText;
	
	private static ScoreManager _instance;
	private static int _comboScore, _hitScore, _maxCombo, _fullComboCount;
	static bool _fullCombo = true;

	void Start()
	{
		_instance = this;
		SongManager.StartGame.AddListener(ResetScore);
		SongManager.EndGame.AddListener(SetEndScore);
	}

	private static void ResetScore()
	{
		_hitScore = 0;
		_comboScore = 0;
		_maxCombo = 0;
		_fullComboCount = 0;
		_fullCombo = true;
	}

	private void SetEndScore()
	{
		maxComboText.text = _fullCombo ? "FULL COMBO" : $"MAX COMBO\n{_maxCombo}";
		hitRatioText.text = $"{_hitScore}/{_fullComboCount}";
	}

	public static void Hit()
	{
		_fullComboCount += 1;

		_hitScore += 1;
		_comboScore += 1;
		if (_comboScore > _maxCombo) _maxCombo = _comboScore;
		_instance.hitSFX.Play();
	}

	public static void Miss()
	{
		_fullComboCount += 1;
		
		_comboScore = 0;
		if (_fullCombo) _fullCombo = !_fullCombo;
		_instance.missSFX.Play();
	}

	private void Update()
	{
		comboText.text = _comboScore.ToString();
	}
}