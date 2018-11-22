using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTextScript : MonoBehaviour
{
	private Text scoreText;

	private void Awake()
	{
		scoreText = GetComponent<Text>();
	}

	private void Start()
	{
		scoreText.text = GameController.GetSavedScore() + "€"; // mostrar el score poco a poco ??
	}
}
