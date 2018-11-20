using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BudScript : MonoBehaviour
{
    /*
    - Cómo adaptar a Android
    - 
     */
    
    [Header("STATE")]
    bool playing;
    [SerializeField] Text txtMain;
    enum BudState { pause, right, left, jump, hurt };
    BudState budState = BudState.pause;

    [Header("CONTROLS")]
    [SerializeField] KeyCode jumpButton;
    [SerializeField] KeyCode shiftButton;

    [Header("POSITION")]
    [SerializeField] LayerMask floorLayer;
    [SerializeField] Transform feet;
    [SerializeField] float radioOverlap = 0.1f;

    [Header("MOVEMENT")]
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 6;
    public float impactForceX = 2;
    public float impactForceY = 5;

    [Header("ENERGY")]
    int maxEnergy = 100;
    [SerializeField] int energy;
    [SerializeField] float dropRatio = 0.1f;
    [SerializeField] Text txtEnergy;

    [Header("SCORE")]
    [SerializeField] int score = 0;
    [SerializeField] Text txtScore;
    
    [Header("OTHER")]
    Animator playerAnimator;
    Rigidbody2D rb2D;

    private void Awake()
	{
        playing = false;
        energy = maxEnergy;

        rb2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
	{
        txtMain.text = "Press any key to continue";
        txtScore.text = "Score: " + score.ToString();

        jumpButton = KeyCode.Z;
        shiftButton = KeyCode.X;

        //GoToCheckpoint();
    }

    private void Update()
	{
        if (!playing)
        {
            if (Input.anyKey)
            {
                playing = true;
                InvokeRepeating("LoseEnergy", 0, dropRatio);
            }
            else return;
        }

        if (energy < 0)
        {
            GoToCheckpoint();
        }

        if (budState == BudState.hurt)
        {
            if (IsGrounded()) budState = BudState.pause;

            return;
        }

        if (Input.GetKey(jumpButton))
        {
            budState = BudState.jump;
        }

        if (Input.GetKey(shiftButton))
        {
            if (budState == BudState.left)
			{
                transform.localScale = new Vector2(1, 1);
                budState = BudState.right;
            }
            else if (budState == BudState.right)
			{
                transform.localScale = new Vector2(-1, 1);
                budState = BudState.left;
            }
            speed = -speed;
        }

        if (budState == BudState.jump)
		{
            budState = BudState.pause;

            if (IsGrounded())
			{
                rb2D.velocity = new Vector2(speed, jumpForce);
            }
			else
			{
                rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
            }
        }
		else
		{
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        }

        if (budState == BudState.right || budState == BudState.left)
		{
            playerAnimator.SetBool("walking", true);
        }
		else
		{
            playerAnimator.SetBool("walking", false);
        }
    }

    private void GoToCheckpoint()
    {
        Vector2 position = GameController.GetPosition();

        if (position != Vector2.zero)
		{
            this.transform.position = position;
        }
    }

    public int GetEnergy()
	{
		return this.energy;
	}

    private void LoseEnergy()
    {
        if (energy > 0) energy--;

        txtEnergy.text = "Energy: " + energy.ToString() + "%";
    }

    public void FillEnergy()
	{
        energy = maxEnergy;
        txtEnergy.text = "Energy: " + energy.ToString() + "%";
        txtScore.text = "Score: " + score.ToString();
    }

    private bool IsGrounded()
	{
        bool isGrounded = false;

        Collider2D collider = Physics2D.OverlapCircle(
			feet.position, radioOverlap, floorLayer);

        if (collider != null) isGrounded = true;

        return isGrounded;
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
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
    }
}