using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{
	int penalty = -5;

	private void Start()
	{
		Destroy(this.gameObject, 3);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Bud")
		{
			other.gameObject.GetComponent<BudScript>().AddScore(penalty);
			
			Destroy(this.gameObject);
		}
	}
}
