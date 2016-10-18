using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Rigidbody2D megaman;

	private Animator myAnimator;

	[SerializeField]
	private float movementSpeed;

	private bool attack;

	private bool facingRight;

	[SerializeField]
	private Transform[] groundPoints;

	[SerializeField]
	private float groundRadius;

	[SerializeField]
	private LayerMask whatIsGround;

	private bool isGrounded;

	private bool jump;

	[SerializeField]
	private float jumpForce;

	[SerializeField]
	private bool airControl;


	// Use this for initialization
	void Start () 
	{
		facingRight = true;
		megaman = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> ();
	}

	void Update()
	{
		HandleInput ();
	}

	// Update is called once per frame
	void FixedUpdate () 
	{

		float horizontal = Input.GetAxis ("Horizontal");

		isGrounded = IsGrounded ();

		HandleMovement (horizontal);

		Flip (horizontal);

		HandleAttacks ();

		ResetValues ();


	}
	
	private void HandleMovement(float horizontal)
	{
		if(!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")&& (isGrounded || airControl))
		{
			megaman.velocity = new Vector2 (horizontal * movementSpeed, megaman.velocity.y);
		}
			
		if (isGrounded && jump) 
		{
			isGrounded = false;
			megaman.AddForce (new Vector2 (0, jumpForce));
		
		}



		myAnimator.SetFloat ("speed",Mathf.Abs(horizontal));
	
	}

	private void HandleAttacks ()
	{
		if (attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) 
		{
			myAnimator.SetTrigger ("attack");
			megaman.velocity = Vector2.zero;
		
		}
	
	}

	private void HandleInput()
	{
		if (Input.GetKeyDown (KeyCode.LeftControl)) 
		{
			attack = true;
		}
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			jump = true;
		}

	}

	private void Flip(float horizontal)
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) 
		{
			facingRight = !facingRight;

			Vector3 theScale = transform.localScale;

			theScale.x *= -1;
			transform.localScale = theScale;
		
		}
	
	}

	private void ResetValues()
	{
		attack = false;
		jump = false;

	}

	private bool IsGrounded()
	{
		if (megaman.velocity.y <= 0) 
		{
			foreach (Transform point in groundPoints) 
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
			
				for (int i = 0; i < colliders.Length; i++) 
				{
					if (colliders [i].gameObject != gameObject) 
					{
						return true;
					
					}

				}

			}
		
		}
		return false;
	}

}
