﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreSubmission : MonoBehaviour {
	public InputField input;
	public Text text; 
	public Highscore[] highscoresList;
	const string privateCode = "_t76kvIZnkqSMK-ecp76CQLbxRBp4CJE6GHdLKmdUV0A";
	const string publicCode = "55553cf46e51b60ed885794f";
	const string webURL = "http://dreamlo.com/lb/";

	void Start ()
	{
		input.onEndEdit.AddListener(SubmitName);  // This also works
		DownloadHighscores ();
	}
	
	private void SubmitName(string arg0)
	{
		AddNewHighscore(arg0, ScoreManager.initialScore);
		text.text = "";
		DownloadHighscores ();
	}

	public void AddNewHighscore(string username, int score) {
		StartCoroutine (UploadNewHighscore (username, score));
	}
	
	IEnumerator UploadNewHighscore(string username, int score) {
		WWW www = new WWW (webURL + privateCode + "/add/" + WWW.EscapeURL (username) + "/" + score);
		yield return www;
		
		if (string.IsNullOrEmpty (www.error))
			print ("Upload Successful");
		else {
			print ("Error Uploading" + www.error);
		}
	}

	public void DownloadHighscores() {
		StartCoroutine ("DownloadHighScoresFromDatabase");
	}
	IEnumerator DownloadHighScoresFromDatabase() {
		WWW www = new WWW (webURL + publicCode + "/pipe/");
		yield return www;
		
		if (string.IsNullOrEmpty (www.error))
			FormatHighscores (www.text);
		else {
			print ("Error Downloading" + www.error);
		}
	}

	public void FormatHighscores(string textStream) {
		string[] entries = textStream.Split (new char[]{'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		highscoresList = new Highscore[entries.Length];
	
		for (int i = 0; i < entries.Length; i++) {
			string[] entryInfo = entries[i].Split (new char[] {'|'});
			string username = entryInfo[0];
			int score = int.Parse (entryInfo[1]);
			highscoresList[i] = new Highscore(username, score);
			text.text += string.Format ("{0,-10} {1,-20}\n", highscoresList[i].username, highscoresList[i].score);
		}
	}
}

public struct Highscore {
	public string username;
	public int score;

	public Highscore( string _username, int _score) {
		username = _username;
		score = _score;
	}
}