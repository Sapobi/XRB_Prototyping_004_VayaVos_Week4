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
	
	private List<Note> notes = new(); //the spawned note prefab
	private List<double> timeStamps = new(); //note spawntimes for this lane

	private int spawnIndex;
	private int inputIndex;
	private bool playing;

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
				var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
				timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
			}
		}
	}

	private void InitializeLane()
	{
		notes = new List<Note>();
		timeStamps = new List<double>();
		spawnIndex = 0;
		inputIndex = 0;
	}

	private void StartLane()
	{
		playing = true;
	}

	void Update()
	{
		if (!playing) return;
		
		//spawn notes
		if (spawnIndex < timeStamps.Count)
		{
			if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
			{
				var noteGameObject = Instantiate(notePrefab, noteSpawn.position, Quaternion.identity);
				var note = noteGameObject.GetComponent<Note>();
				notes.Add(note);
				note.assignedTime = (float)timeStamps[spawnIndex];
				note.spawnPos = noteSpawn.position;
				note.tapPos = transform.position;
				note.despawnPos = noteDespawn.position;
				spawnIndex++;
			}
		}

		//miss notes
		if (inputIndex < timeStamps.Count)
		{
			var timeStamp = timeStamps[inputIndex];
			var marginOfError = SongManager.Instance.marginOfError;
			var audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

			if (timeStamp + marginOfError <= audioTime)
			{
				Miss();
				print($"Missed {inputIndex} note");
				inputIndex++;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		HandleTapFeedback();
		if (!playing) return;
		
		//hit notes
		if (inputIndex < timeStamps.Count)
		{
			var timeStamp = timeStamps[inputIndex];
			var marginOfError = SongManager.Instance.marginOfError;
			var audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

			if (Math.Abs(audioTime - timeStamp) < marginOfError)
			{
				Hit();
				print($"Hit on {inputIndex} note");
				Destroy(notes[inputIndex].gameObject);
				inputIndex++;
			}
			else
			{
				print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
			}
		}
	}

	private void HandleTapFeedback()
	{
		tapSound.Play();
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