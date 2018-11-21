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
    enum BudState { pause, walk, jump, hurt }
    enum BudDirection { right, left }
    BudState budState = BudState.pause;
    BudDirection budDirection = BudDirection.right;

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

        jumpButton = KeyCode.X;
        shiftButton = KeyCode.Z;

        //GoToCheckpoint();
    }

    private void Update()
	{
        if (!playing)
        {
            if (Input.anyKey)
            {
                playing = true;
                budState = BudState.walk;
                InvokeRepeating("LoseEnergy", 0, dropRatio);
            }
            else return;
        }

        if (energy < 1)
        {
            //GoToCheckpoint();
        }
    }

    private void FixedUpdate()
    {
        CheckState();
        CheckInputs();
    }

    private void CheckState()
    {
        switch (budState)
        {
            case BudState.hurt:

                playerAnimator.SetBool("walking", false);

                if (IsGrounded()) budState = BudState.walk;

                return;

            case BudState.jump:

                playerAnimator.SetBool("walking", false);

                if (IsGrounded())
                {
                    rb2D.velocity = new Vector2(speed, jumpForce);
                }
                else rb2D.velocity = new Vector2(speed, rb2D.velocity.y);

                budState = BudState.pause;

                break;

            case BudState.pause:

                if (IsGrounded())
                {
                    budState = BudState.walk;
                }

                break;

            case BudState.walk:

                playerAnimator.SetBool("walking", true);

                rb2D.velocity = new Vector2(speed, rb2D.velocity.y);

                break;
        }
    }

    private void CheckInputs()
    {
        if (Input.GetKey(jumpButton))
        {
            budState = BudState.jump;
        }

        if (Input.GetKeyDown(shiftButton))
        {
            if (budDirection == BudDirection.left)
            {
                transform.localScale = new Vector2(1, 1);
                budDirection = BudDirection.right;
            }
            else if (budDirection == BudDirection.right)
            {
                transform.localScale = new Vector2(-1, 1);
                budDirection = BudDirection.left;
            }
            speed = -speed;
        }
    }

    private void GoToCheckpoint()
    {
        Vector2 position = GameController.GetPosition();

        if (position != Vector2.zero)
		{
            this.transform.position = position;
        }
        playing = false;
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
    }

    public void AddScore(int plus)
    {
        score += plus;
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
        if (collision.gameObject.tag == "Enemy")
        {
            if (budDirection == BudDirection.right)
            {
                GetComponent<Rigidbody2D>().AddRelativeForce(
                    new Vector2(-impactForceX, impactForceY), ForceMode2D.Impulse);
            }
            else if (budDirection == BudDirection.left)
            {
                GetComponent<Rigidbody2D>().AddRelativeForce(
                    new Vector2(impactForceX, impactForceY), ForceMode2D.Impulse);
            }
            budState = BudState.hurt;
        }
    }
}
