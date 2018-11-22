using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Bud")
		{
			GameController.StoreScore(
				other.gameObject.GetComponent<BudScript>().GetScore()); // no guarda el score entre escenas ??
				
			GameController.LoadNextScene();
		}
	}
}
