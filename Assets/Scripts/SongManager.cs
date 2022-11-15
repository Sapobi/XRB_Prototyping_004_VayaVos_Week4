using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class SongManager : MonoBehaviour
{
	[SerializeField] private Lane[] lanes;

	public static SongManager Instance;

	public double marginOfError; // in seconds
	public int inputDelayInMilliseconds;
	public float noteTime;

	//song
	public AudioSource audioSource;
	public string songName;
	public int songDiff;
	public MidiFile MidiFile;

	public static readonly UnityEvent StartGame = new();
	public static readonly UnityEvent EndGame = new();

	// Start is called before the first frame update
	void Start()
	{
		Instance = this;
		StartGame.AddListener(StartSong);
	}

	private void StartSong()
	{
		ReadFromFile();
		audioSource.Play();
	}

	private void ReadFromFile()
	{
		MidiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + songName + songDiff + ".mid");
		GetDataFromMidi();
	}

	private void GetDataFromMidi()
	{
		var notes = MidiFile.GetNotes();
		var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
		notes.CopyTo(array, 0);

		foreach (var lane in lanes) lane.SetTimeStamps(array);
	}

	public static double GetAudioSourceTime()
	{
		return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
	}
}