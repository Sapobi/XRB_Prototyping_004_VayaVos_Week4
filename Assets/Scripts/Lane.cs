using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
	public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction; //coressponding note to the lane
	public KeyCode input; //the button that needs to be pressed for this lane
	public GameObject notePrefab; //will move this to Note instead, as depending on the note type you will need different prefab later
	List<Note> notes = new(); //the spawned notes
	public List<double> timeStamps = new();
	public Transform noteSpawn;
	public Transform noteTap;

	int spawnIndex = 0;
	int inputIndex = 0;

	public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
	{
		foreach (var note in array)
		{
			if (note.NoteName == noteRestriction)
			{
				var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
				timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
			}
		}
	}

	void Update()
	{
		if (spawnIndex < timeStamps.Count)
		{
			if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
			{
				var note = Instantiate(notePrefab, noteSpawn.position, Quaternion.identity);
				notes.Add(note.GetComponent<Note>());
				note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
				note.GetComponent<Note>().spawnPos = noteSpawn.position;
				note.GetComponent<Note>().tapPos = noteTap.position;
				spawnIndex++;
			}
		}

		if (inputIndex < timeStamps.Count)
		{
			double timeStamp = timeStamps[inputIndex];
			double marginOfError = SongManager.Instance.marginOfError;
			double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

			if (Input.GetKeyDown(input)) //need to change to actual vr input
			{
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

			if (timeStamp + marginOfError <= audioTime)
			{
				Miss();
				print($"Missed {inputIndex} note");
				inputIndex++;
			}
		}
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