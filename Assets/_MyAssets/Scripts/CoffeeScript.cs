using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeScript : MonoBehaviour
{
	[SerializeField] int scoreBonus = 10;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Bud")
		{
			BudScript budScript =
                other.gameObject.GetComponent<BudScript>();

			budScript.FillEnergy();
			budScript.AddScore(scoreBonus);
			
			Destroy(this.gameObject);
		}
	}
}
