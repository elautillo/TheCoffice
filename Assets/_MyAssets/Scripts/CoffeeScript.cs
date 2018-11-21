using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeScript : MonoBehaviour
{
	[SerializeField] BudScript budScript;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Bud")
		{
			budScript.FillEnergy();
			budScript.AddScore(10);
			
			Destroy(this.gameObject);
		}
	}
}
