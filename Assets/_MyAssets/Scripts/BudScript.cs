using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BudScript : MonoBehaviour
{
    [Header("STATE")]
    [SerializeField] Text txtMain;
    enum BudState { pause, walk, jump, hurt }
    enum BudDirection { right, left }
    BudState budState;
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
    [SerializeField] bool losingEnergy = false;
    [SerializeField] Text txtEnergy;

    [Header("SCORE")]
    [SerializeField] int score;
    [SerializeField] Text txtScore;
    
    [Header("OTHER")]
    Animator playerAnimator;
    Rigidbody2D rb2D;
    
    private void Awake()
	{
        GameController.SetPlay(false);
        energy = maxEnergy;
        jumpButton = KeyCode.X;
        shiftButton = KeyCode.Z;

        rb2D = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Start()
	{
        txtEnergy.text = "Energy: " + energy;

        GameController.GetSavedScore();
        txtScore.text = "Score: " + score;

        GameController.ClearData();

        GoToCheckpoint();
    }

    private void Update()
	{
        if (!GameController.IsPlaying())
        {
            budState = BudState.pause; // sigue caminando al pasar un checkpoint ??
            txtMain.text = "Press any key to continue";

            if (Input.anyKey)
            {
                GameController.SetPlay(true);
                budState = BudState.walk;
                txtMain.text = "";
                
                if (!losingEnergy)
                {
                    InvokeRepeating("LoseEnergy", 0, dropRatio); //cada vez mas rapido al pasar por checkpoint ??
                    losingEnergy = true;
                }
            }
            else return;
        }

        /* if (energy < 1)
        {
            losingEnergy = false;
            GoToCheckpoint();
            FillEnergy(); // cambiar
        } */
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
                    GetComponent<AudioSource>().Play();
                }
                else rb2D.velocity = new Vector2(speed, rb2D.velocity.y);

                budState = BudState.pause;

                break;

            case BudState.pause:

                playerAnimator.SetBool("walking", false);

                if (GameController.IsPlaying() && IsGrounded())
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
        this.transform.position = GameController.GetPosition();
    }

    public int GetEnergy()
	{
		return this.energy;
	}

    private void LoseEnergy()
    {
        if (energy > 0) energy--;

        txtEnergy.text = "Energy: " + energy + "%";
    }

    public void FillEnergy()
	{
        energy = maxEnergy;
        txtEnergy.text = "Energy: " + energy + "%";
    }

    public int GetScore()
    {
        return this.score;
    }

    public void SetScore(int number)
    {
        score = number;
        txtScore.text = "Score: " + score;
    }

    public void AddScore(int bonus)
    {
        score += bonus;
        txtScore.text = "Score: " + score;
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
