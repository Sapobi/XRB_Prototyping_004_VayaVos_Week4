using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
	[SerializeField] private Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction; //corresponding note to the lane
	[SerializeField] private GameObject notePrefab; //will have to move this to Note instead, as depending on the note type you will need different prefab later
	[SerializeField] private Transform noteSpawn, noteDespawn;
	[SerializeField] private AudioSource tapSound;

	private List<Note> _notes = new(); //the spawned note prefab
	private List<double> _timeStamps = new(); //note spawntimes for this lane

	private int _spawnIndex;
	private int _inputIndex;
	private bool _playing;

	private void Start()
	{
		SongManager.StartGame.AddListener(StartLane);
	}

	public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
	{
		InitializeLane();
		foreach (var note in array)
		{
			if (note.NoteName == noteRestriction)
			{
				var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.Instance.MidiFile.GetTempoMap());
				_timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
			}
		}
	}

	private void InitializeLane()
	{
		_notes = new List<Note>();
		_timeStamps = new List<double>();
		_spawnIndex = 0;
		_inputIndex = 0;
	}

	private void StartLane()
	{
		_playing = true;
	}

	void Update()
	{
		if (!_playing) return;

		//spawn notes
		if (_spawnIndex < _timeStamps.Count)
		{
			if (SongManager.GetAudioSourceTime() >= _timeStamps[_spawnIndex] - SongManager.Instance.noteTime)
			{
				var spawnPosition = noteSpawn.position;
				var noteGameObject = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
				var note = noteGameObject.GetComponent<Note>();
				_notes.Add(note);
				note.assignedTime = (float)_timeStamps[_spawnIndex];
				note.spawnPos = spawnPosition;
				note.tapPos = transform.position;
				note.despawnPos = noteDespawn.position;
				_spawnIndex++;
			}
		}

		//miss notes
		if (_inputIndex < _timeStamps.Count)
		{
			var timeStamp = _timeStamps[_inputIndex];
			var marginOfError = SongManager.Instance.marginOfError;
			var audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

			if (timeStamp + marginOfError <= audioTime)
			{
				Miss();
				print($"Missed {_inputIndex} note");
				_inputIndex++;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		HandleTapFeedback();
		if (!_playing) return;

		//hit notes
		if (_inputIndex < _timeStamps.Count)
		{
			var timeStamp = _timeStamps[_inputIndex];
			var marginOfError = SongManager.Instance.marginOfError;
			var audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

			if (Math.Abs(audioTime - timeStamp) < marginOfError)
			{
				Hit();
				print($"Hit on {_inputIndex} note");
				Destroy(_notes[_inputIndex].gameObject);
				_inputIndex++;
			}
			else
			{
				print($"Hit inaccurate on {_inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
			}
		}
	}

	private void HandleTapFeedback()
	{
		tapSound.Play();
		//change material
		//haptic feedback
	}

	private void Hit()
	{
		ScoreManager.Hit();
	}

	private void Miss()
	{
		ScoreManager.Miss();
	}
}