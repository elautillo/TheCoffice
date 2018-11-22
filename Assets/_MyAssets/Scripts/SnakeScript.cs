using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{
	private Transform bud;
	[SerializeField] GameObject slimePrefab;
	private GameObject slime;
	float speed = 0.5f;
	[SerializeField] float attackSpeed = 2f;
	float shotCadence;

	private void Start()
	{
		bud = GameObject.Find("Bud").transform;
		shotCadence = attackSpeed;
	}

	private void FixedUpdate()
	{
		transform.right = -(bud.position - transform.position);
		TryShoot();
	}

	void TryShoot()
	{
		shotCadence += Time.deltaTime;

		if (shotCadence >= attackSpeed)
        {
            shotCadence = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        slime = Instantiate(slimePrefab, transform.position, Quaternion.identity);
        slime.gameObject.GetComponent<Rigidbody2D>().velocity = (bud.position - transform.position * speed);
    }
}
