using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
	[SerializeField] int speed = 10;
	Rigidbody2D rb2D;

	private void Awake()
	{
		rb2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		rb2D.velocity = new Vector2(speed, 0);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		print(collision.gameObject.name); // no colisionan con el grid ??
		if (collision.gameObject.layer == 8)
		{
			speed = -speed;
		}
	}
}
