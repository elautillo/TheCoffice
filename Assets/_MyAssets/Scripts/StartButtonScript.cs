using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{
	private Button button;

	private void Awake()
	{
		button = GetComponent<Button>();
	}

	private void Start()
	{
		button.onClick.AddListener(StartGame);
	}

	private void StartGame()
	{
		GameController.ClearData();
		SceneManager.LoadScene(1);
	}
}
