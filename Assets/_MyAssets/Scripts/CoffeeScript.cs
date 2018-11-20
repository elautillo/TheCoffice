using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeScript : MonoBehaviour
{
	[SerializeField] BudScript budScript;

	private void OnTriggerEnter(Collider other)
	{
		string name = other.gameObject.name;

		if (name == "Bud" || name == "Feet")
		{
			print("jelou");
			budScript.FillEnergy();
			
			Destroy(this.gameObject);
		}
	}
}
