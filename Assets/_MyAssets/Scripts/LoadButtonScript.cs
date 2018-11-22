using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadButtonScript : MonoBehaviour
{
	private Button button;

	private void Awake()
	{
		button = GetComponent<Button>();
		button.interactable = false;
	}

	private void Start()
	{
		if (GameController.GetSavedScene() != -1)
		{
			button.onClick.AddListener(LoadGame);
			button.interactable = true;
		}
	}

	private void LoadGame()
	{
		GameController.LoadSavedScene();
	}
}
