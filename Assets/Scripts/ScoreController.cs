using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
	public Text Score;
	public Text FinalScore;
	public Text BestScore;

	public BirdController Bird;

	private int _Score;

	private void Start()
	{
		_Score = 0;

		Bird.AddPointReachedListener(OnPointReached);
		Bird.AddGameOverListener(OnGameOver);
	}

	private void OnPointReached()
	{
		_Score += 1;
		Score.text = _Score.ToString();
	}

	private void OnGameOver()
	{
		FinalScore.text = $"Score {_Score}";

		int bestScore = PlayerPrefs.GetInt("BestScore", 0);
		if (bestScore < _Score)
		{
			bestScore = _Score;
			PlayerPrefs.SetInt("BestScore", bestScore);
		}

		BestScore.text = $"Best score {bestScore}";

		Score.gameObject.SetActive(false);
	}
}
