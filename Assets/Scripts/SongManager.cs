using System.Collections;
using System.IO;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

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

	private bool _playing;
	private string _filePath;
	

	// Start is called before the first frame update
	void Awake()
	{
		Instance = this;
		StartGame.AddListener(StartSong);
		EndGame.AddListener(() => SetPlaying(false));
	}

	private void Update()
	{
		if (!_playing) return;
		if (audioSource.isPlaying) return;
		
		EndGame.Invoke();
	}

	private void StartSong()
	{
		_filePath = Application.streamingAssetsPath + "/" + songName + songDiff + ".mid";
		
		if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
		{
			StartCoroutine(ReadFromWebsite());
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			ReadAndroid();
		}
		else
		{
			ReadFromFile();
		}
		audioSource.Play();
		SetPlaying(true);
	}

	private void SetPlaying(bool state)
	{
		_playing = state;
	}

	private IEnumerator ReadFromWebsite()
	{
		using (var www = UnityWebRequest.Get(_filePath))
		{
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.LogError(www.error);
			}
			else
			{
				byte[] results = www.downloadHandler.data;
				using (var stream = new MemoryStream(results))
				{
					MidiFile = MidiFile.Read(stream);
					GetDataFromMidi();
				}
			}
		}
	}
	
	private void ReadAndroid()
	{
		var www = UnityWebRequest.Get(_filePath);
		www.SendWebRequest();
		while (!www.isDone)
		{
		}
		var results = www.downloadHandler.data;
		using (var stream = new MemoryStream(results))
		{
			MidiFile = MidiFile.Read(stream);
			GetDataFromMidi();
		}
	}

	private void ReadFromFile()
	{
		MidiFile = MidiFile.Read(_filePath);
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