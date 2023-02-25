using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public bool isGrounded;

	[HideInInspector]
	public bool isClimbing;

	public bool isTalking;

	public float groundRemember;

	public float groundRememberTimer;

	public float jumpPressedRemember;

	public float jumpPressedRememberTimer;

	public float cutJumpHeight;

	public bool isJumping;

	public float jumpForce;

	public Transform groundCheck;

	public float groundCheckRadius;

	public LayerMask[] collisionLayers;

	public Animator animator;

	public float vitesse;

	public float moveSpeed;

	public float climbSpeed;

	public Rigidbody2D rb;

	public SpriteRenderer spriteRenderer;

	private Vector3 velocity = Vector3.zero;

	private float horizontalMovement;

	private float verticalMovement;

	public CapsuleCollider2D playerCollider;

	public ParticleSystem footSteps;

	private ParticleSystem.EmissionModule footEmission;

	public float dodgeSpeed;

	public float dodgeTimer;

	public float startDodgeTime;

	public float dodgeCount;

	public float dodgeFrame;

	public bool isDodging;

	public float climbingHorizontalOffset;

	private Vector2 topOfPlayer;

	private GameObject ledge;

	private bool falling;

	private bool moved;

	public bool grabbingLedge;

	public float timeHangB4Jump;

	public float timeHangB4JumpTimer;

	public bool onlyVerticalMvmt;

	public static PlayerMovement instance;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instancede PlayerMovement dans la scène");
			return;
		}
		instance = this;
		vitesse = moveSpeed;
		dodgeTimer = startDodgeTime;
	}

	private void Start()
	{
		footEmission = footSteps.emission;
	}

	private void Update()
	{
		if (PlayerHealth.instance.currentHealth > 0f) // Player alive
		{
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -50f, 9f));
		}
		if (!isDodging)//Shift not pressed
		{
			CheckForLedge();
			LedgeHanging();
		}
		if (Input.GetKeyDown(KeyCode.LeftShift) && !grabbingLedge) // Shift Press
		{
			Debug.Log("Shift");
			if (dodgeTimer <= 0f)
			{
				dodgeCount = dodgeFrame;
			}
		}
		if (dodgeCount != 0f) // Dodging
		{
			if (!isDodging)
			{
				animator.SetTrigger("Dodge");
				isDodging = true;
				StartCoroutine(InvincibilityDash());
			}
			dodgeCount -= 1f;
			if (spriteRenderer.flipX)
			{
				rb.velocity = new Vector2(0f - dodgeSpeed, rb.velocity.y);
				Debug.Log("Gauche");
			}
			else
			{
				rb.velocity = new Vector2(dodgeSpeed, rb.velocity.y);
				Debug.Log("Droite");
			}
			dodgeTimer = startDodgeTime;
		}
		else
		{
			isDodging = false;
		}
		dodgeTimer -= Time.deltaTime;
		jumpPressedRemember -= Time.deltaTime;
		groundRemember -= Time.deltaTime;
		timeHangB4Jump -= Time.deltaTime;
		//Jump
		if (Input.GetButtonDown("Jump"))
		{
			jumpPressedRemember = jumpPressedRememberTimer;
		}
		if (Input.GetButtonUp("Jump") && !falling && rb.velocity.y > 0f)
		{
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * cutJumpHeight);
		}
		if (isGrounded)
		{
			groundRemember = groundRememberTimer;
		}
		if (jumpPressedRemember > 0f && groundRemember > 0f && !isClimbing && !isTalking)
		{
			jumpPressedRemember = 0f;
			groundRemember = 0f;
			isJumping = true;
		}
		if (Input.GetAxisRaw("Horizontal") != 0f && isGrounded)
		{
			Debug.Log("FootSteps");
			footSteps.Play();
			footEmission.rateOverTime = 70f;
		}
		else
		{
			Debug.Log("NoFootSteps");
			footEmission.rateOverTime = 0f;
			footSteps.Stop();
		}
		//Climbing
		Flip(rb.velocity.x);
		horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
		verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;
		float value = Mathf.Abs(rb.velocity.x);
		animator.SetFloat("Speed", value);
		animator.SetBool("isClimbing", isClimbing);
		animator.SetBool("isJumping", isJumping);
		animator.SetBool("isGrounded", isGrounded);
	}

	private void FixedUpdate()
	{
		isGrounded = (bool)Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers[0]) || (bool)Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers[1]);
		MovePlayer(horizontalMovement, verticalMovement, 0.05f);
	}

	private void MovePlayer(float _horizontalMovement, float _verticalMovement, float _smoothTime) // Movement
	{
		if (isTalking)
		{
			return;
		}
		if (!isClimbing)
		{
			if (!onlyVerticalMvmt)
			{
				Vector3 target = new Vector2(_horizontalMovement, rb.velocity.y);
				rb.velocity = Vector3.SmoothDamp(rb.velocity, target, ref velocity, _smoothTime);
			}
			if (isJumping)
			{
				rb.AddForce(new Vector2(0f, jumpForce));
				isJumping = false;
			}
		}
		else
		{
			Vector3 target2 = new Vector2(0f, _verticalMovement);
			rb.velocity = Vector3.SmoothDamp(rb.velocity, target2, ref velocity, _smoothTime);
		}
	}
	// Flip the sprite
	private void Flip(float _velocity)
	{
		if (!grabbingLedge)
		{
			if (horizontalMovement > 0f)
			{
				spriteRenderer.flipX = false;
			}
			else if (horizontalMovement < 0f)
			{
				spriteRenderer.flipX = true;
			}
		}
	}

	public IEnumerator InvincibilityDash() // Invincibility during dodge
	{
		PlayerHealth.instance.isInvincible = true;
		yield return new WaitForSeconds(0.5f);
		PlayerHealth.instance.isInvincible = false;
	}

	public void CheckForLedge() // Possibility to grab a ledge
	{
		if (falling)
		{
			return;
		}
		if (!spriteRenderer.flipX)
		{
			// Check for ledge on the right
			topOfPlayer = new Vector2(playerCollider.bounds.max.x + 0.1f, playerCollider.bounds.max.y);
			RaycastHit2D raycastHit2D = Physics2D.Raycast(topOfPlayer, Vector2.right, 0.2f);
			if ((bool)raycastHit2D && (bool)raycastHit2D.collider.gameObject.GetComponent<Ledge>()) // If there is a ledge
			{
				ledge = raycastHit2D.collider.gameObject;
				if (playerCollider.bounds.max.y < ledge.GetComponent<Collider2D>().bounds.max.y && playerCollider.bounds.max.y > ledge.GetComponent<Collider2D>().bounds.center.y) // If the player is close enough to the ledge
				{
					if (!grabbingLedge)
					{
						timeHangB4Jump = timeHangB4JumpTimer;
					}
					grabbingLedge = true;
					animator.SetBool("Hang", grabbingLedge);
				}
			}
		}
		else
		{
			topOfPlayer = new Vector2(playerCollider.bounds.min.x - 0.1f, playerCollider.bounds.max.y);
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast(topOfPlayer, Vector2.left, 0.2f);
			if ((bool)raycastHit2D2 && (bool)raycastHit2D2.collider.gameObject.GetComponent<Ledge>())
			{
				ledge = raycastHit2D2.collider.gameObject;
				if (playerCollider.bounds.max.y < ledge.GetComponent<Collider2D>().bounds.max.y && playerCollider.bounds.max.y > ledge.GetComponent<Collider2D>().bounds.center.y)
				{
					if (!grabbingLedge)
					{
						timeHangB4Jump = timeHangB4JumpTimer;
					}
					grabbingLedge = true;
					animator.SetBool("Hang", value: true);
				}
			}
		}
		if (ledge != null && grabbingLedge) 
		{
			AdjustPlayerPosition();
			rb.velocity = Vector2.zero;
			rb.bodyType = RigidbodyType2D.Kinematic;
			onlyVerticalMvmt = true;
		}
		else
		{
			onlyVerticalMvmt = false;
			rb.bodyType = RigidbodyType2D.Dynamic;
		}
	}

	public void LedgeHanging() // When grabbed a ledge
	{
		if (grabbingLedge && Input.GetKey(KeyCode.Space) && timeHangB4Jump < 0f)
		{
			Debug.Log("jump");
			animator.SetBool("LedgeClimbing", value: true);
			animator.SetBool("Hang", value: false);
			rb.bodyType = RigidbodyType2D.Dynamic;
			rb.AddForce(new Vector2(0f, 2f * jumpForce));
			ledge = null;
			falling = true;
			moved = false;
			grabbingLedge = false;
			Invoke("NotFalling", 0.5f);
		}
		if (grabbingLedge && Input.GetAxis("Vertical") < 0f)
		{
			ledge = null;
			moved = false;
			grabbingLedge = false;
			animator.SetBool("Hang", value: false);
			falling = true;
			rb.bodyType = RigidbodyType2D.Dynamic;
			onlyVerticalMvmt = false;
			Invoke("NotFalling", 0.5f);
		}
	}

	public void AdjustPlayerPosition() // Adjust the player position when grabbed a ledge
	{
		if (!moved)
		{
			moved = true;
			if (!spriteRenderer.flipX)
			{
				base.transform.position = new Vector2(ledge.GetComponent<Collider2D>().bounds.min.x - playerCollider.bounds.extents.x + ledge.GetComponent<Ledge>().hangingHorizontalOffset, ledge.GetComponent<Collider2D>().bounds.max.y - playerCollider.bounds.extents.y - 0.5f + ledge.GetComponent<Ledge>().hangingVerticalOffset);
			}
			else
			{
				base.transform.position = new Vector2(ledge.GetComponent<Collider2D>().bounds.max.x + playerCollider.bounds.extents.x - ledge.GetComponent<Ledge>().hangingHorizontalOffset, ledge.GetComponent<Collider2D>().bounds.max.y - playerCollider.bounds.extents.y - 0.5f + ledge.GetComponent<Ledge>().hangingVerticalOffset);
			}
		}
	}

	protected virtual void NotFalling()
	{
		falling = false;
		animator.SetBool("LedgeClimbing", value: false);
	}
}
