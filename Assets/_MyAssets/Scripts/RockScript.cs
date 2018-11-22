using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "Bud")
		{
			collision.gameObject.GetComponent<BudScript>().AddScore(-10);
		}
	}
}
