using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class SongManager : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private Lane[] lanes;
	
	public static SongManager Instance;
	
	public double marginOfError; // in seconds
	public int inputDelayInMilliseconds;
	public float noteTime;
	
	//midifile name
	public string fileLocation; 
	public static MidiFile midiFile;

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
		midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
		GetDataFromMidi();
	}

	private void GetDataFromMidi()
	{
		var notes = midiFile.GetNotes();
		var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
		notes.CopyTo(array, 0);

		foreach (var lane in lanes) lane.SetTimeStamps(array);
	}

	public static double GetAudioSourceTime()
	{
		return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
	}
}