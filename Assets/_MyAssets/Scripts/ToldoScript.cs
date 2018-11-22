using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToldoScript : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "Bud")
		{
			Invoke("Dismantle", 1);
		}
	}

	private void Dismantle()
	{
		this.gameObject.SetActive(false);
	}
}
