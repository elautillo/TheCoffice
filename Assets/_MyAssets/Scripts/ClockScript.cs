using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
	int counter = 0;
	bool counting = false;
	BudScript bud;
	int score;
	ParticleSystem power;

	private void Start()
	{
		power = GameObject.Find("Power").GetComponent<ParticleSystem>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Bud")
		{
			bud = other.gameObject.GetComponent<BudScript>();
			score = bud.GetScore();
			counting = true;
		}
	}

	private void FixedUpdate()
	{
		if (counting)
		{
			if (bud.GetScore() < score)
			{
				bud.SetScore(score);
			}
			counter++;
			power.Play();
		}

		if (counter > 400)
		{
			counting = false;
			counter = 0;
		}
	}
}
