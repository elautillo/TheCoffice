using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BudScript : MonoBehaviour {

    enum BudState { pause, right, left, jump, hurt };
    BudState budState = BudState.pause;

    [SerializeField] LayerMask floorLayer;
    [SerializeField] Transform feetPos;
    [SerializeField] Text txtScore;
    [SerializeField] Text txtEnergy;
    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = 1;

    int maxEnergy = 4;
    [SerializeField] int energy;
    [SerializeField] UIScript uiScript;

    [SerializeField] int score = 0;
    [SerializeField] float radioOverlap = 0.1f;
    Animator playerAnimator;
    Rigidbody2D rb2D;

    public float impactForceX = 4;
    public float impactForceY = 4;

    private void Awake()
	{
        energy = maxEnergy;
    }

    private void Start()
	{
        rb2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

        txtScore.text = "Score:" + score.ToString();
        txtEnergy.text = "Energy:" + energy.ToString();

        //Recupera la posición del último checkpoint
        Vector2 position = GameController.GetPosition();

        if (position != Vector2.zero)
		{
            this.transform.position = position;
        }
    }

    private void Update()
	{
        if (Input.GetKey(KeyCode.Space))
		{
            budState = BudState.jump;
        }

        if (budState == BudState.hurt && OnTheGround())
		{
            budState = BudState.pause;
        }
    }

    void FixedUpdate()
	{
        float xPos = Input.GetAxis("Horizontal");
        float yCurrentSpeed = rb2D.velocity.y;
        
        if (budState == BudState.hurt)
		{
            return;
        }

        if (Mathf.Abs(xPos) > 0.01f)
		{
            playerAnimator.SetBool("walking", true);
        }
		else
		{
            playerAnimator.SetBool("walking", false);
        }

        if (budState == BudState.jump)
		{
            budState = BudState.pause;

            if (OnTheGround())
			{
                rb2D.velocity = new Vector2(xPos * speed, jumpForce);
            }
			else
			{
                rb2D.velocity = new Vector2(xPos * speed, yCurrentSpeed);
            }
        }
		else if (xPos > 0.01f)
		{
            rb2D.velocity = new Vector2(xPos * speed, yCurrentSpeed);

            if (budState == BudState.left)
			{
                transform.localScale = new Vector2(1, 1);
            }
            budState = BudState.right;
        }
		else if (xPos < -0.01f)
		{
            rb2D.velocity = new Vector2(xPos * speed, yCurrentSpeed);

            if (budState == BudState.right)
			{
                transform.localScale = new Vector2(-1, 1);
            }
            budState = BudState.left;
        }
    }

    public void FillEnergy()
	{
        energy = maxEnergy;
        txtScore.text = "Score: " + score.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("Coffee"))
		{
            FillEnergy();
            Destroy(collision.gameObject);
        }
    }

    private bool OnTheGround()
	{
        bool enSuelo = false;
        Collider2D colider = Physics2D.OverlapCircle(
			feetPos.position, radioOverlap, floorLayer);

        if (colider != null)
		{
            enSuelo = true;
        }
        return enSuelo;
    }

    public void RecibirDanyo(int danyo)
	{
        energy = energy - danyo;

        if (energy <= 0)
		{
            uiScript.RestarVida();
            energy = maxEnergy;
        }
        if (budState == BudState.right)
		{
            GetComponent<Rigidbody2D>().AddRelativeForce(
            	new Vector2(-impactForceX, impactForceY), ForceMode2D.Impulse);

            budState = BudState.hurt;
        }
		else if (budState == BudState.left)
		{
            GetComponent<Rigidbody2D>().AddRelativeForce(
            	new Vector2(impactForceX, impactForceY), ForceMode2D.Impulse);

            budState = BudState.hurt;
        }
        txtEnergy.text = "Health:" + energy.ToString();
    }

	public int GetEnergy()
	{
		return this.energy;
	}

    /*
    //Version basada en TAG y utilizando OverlapCircleAll
    private bool OnTheGround() {
    bool enSuelo = false;
    Collider2D[] cols = Physics2D.OverlapCircleAll(feetPos.position, radioOverlap);
    for (int i = 0; i < cols.Length; i++) {
    if (cols[i].gameObject.tag == "Suelo") {
    enSuelo = true;
    break;
    }
    }
    return enSuelo;
    }
    */
}