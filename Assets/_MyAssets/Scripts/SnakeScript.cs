using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{
	int counter = 0;
	bool counting = false;
	private Transform bud;
	[SerializeField] GameObject slimePrefab;
	private GameObject slime;

	private void Start()
	{
		bud = GameObject.Find("Bud").transform;
	}

	private void Update()
	{
		transform.right = -(bud.position - transform.position);
		slime = Instantiate(slimePrefab, transform.position, Quaternion.identity);

		if (counting)
		{
			counter++;
		}
		else
		{
			slime.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-5, 0);
			counting = true;
		}

		if (counter > 100)
		{
			counting = false;
			counter = 0;
		}
	}
}
