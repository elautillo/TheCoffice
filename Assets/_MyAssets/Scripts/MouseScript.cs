using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseScript : MonoBehaviour
{
    public Transform rightLimit;
    public Transform leftLimit;
    bool goingRight = false;

    private void Start()
    {
        transform.position = rightLimit.position;
    }
    
    void Update()
    {
        if (goingRight == true)
        {
            transform.Translate(Vector2.right * Time.deltaTime);

            if (transform.position.x > rightLimit.position.x)
            {
                goingRight = false;
                ChangeDirection();
            }
        }
        else
        {
            transform.Translate(Vector2.left* Time.deltaTime);

            if (transform.position.x < leftLimit.position.x)
            {
                goingRight = true;
                ChangeDirection();
            }
        }
	}

    private void ChangeDirection()
    {
        if (goingRight)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }
    }
}
