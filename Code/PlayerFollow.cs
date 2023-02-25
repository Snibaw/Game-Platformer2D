using System.Collections;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
	public int damageOnCollision;

	public Transform borderLeft;

	public Transform borderRight;

	public float speed;

	public Transform target;

	private SpriteRenderer graphics;

	private Animator animator;

	private EnnemyHealth ennemyHealth;

	private bool Walk;

	private bool inside;

	public float vitesse;

	private Rigidbody2D rb;

	public bool isAttacking;

	public bool isKing;

	public float outOfBorder;

	private bool borderOut;

	private Transform borderNear;

	public float stoppingDistance;

	private void Start()
	{
		// Initialisation
		ennemyHealth = base.gameObject.GetComponent<EnnemyHealth>();
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		rb = base.gameObject.GetComponent<Rigidbody2D>();
		animator = base.gameObject.GetComponent<Animator>();
		graphics = base.gameObject.GetComponent<SpriteRenderer>();
		vitesse = speed;
	}

	private void Update()
	{
		outOfBorder -= Time.deltaTime;
		rb.velocity = Vector3.zero;
		if (PlayerHealth.instance.currentHealth <= 0f) // If player is Dead
		{
			animator.SetBool("Walk", value: false);
		}
		else
		{
			if (ennemyHealth.isDead) // If ennemy is Dead
			{
				return;
			}
			FlipEnnemy(_flip: false);
			if (outOfBorder < 0f && borderOut) // If ennemy out of border
			{
				FindNearestBorder();
				base.transform.position = new Vector3(borderNear.position.x, borderLeft.position.y, borderLeft.position.z);
			}
			if (base.transform.position.x < borderLeft.position.x) // If ennemy is on the left border
			{
				if (!borderOut)
				{
					borderOut = true;
					outOfBorder = 0.5f;
				}
				base.transform.Translate(new Vector3(1f * speed * Time.deltaTime, 0f, 0f), Space.World);
				animator.SetBool("walk", value: true);
				graphics.flipX = false;
			}
			else if (base.transform.position.x > borderRight.position.x) // If ennemy is on the right border
			{
				if (!borderOut)
				{
					borderOut = true;
					outOfBorder = 5f;
				}
				base.transform.Translate(new Vector3(-1f * speed * Time.deltaTime, 0f, 0f), Space.World);
				animator.SetBool("walk", value: true);
				graphics.flipX = true;
			}
			else if (Mathf.Abs(target.position.x - base.transform.position.x) < stoppingDistance) // If ennemy is near the player
			{
				borderOut = false;
				Vector3 vector = target.position - base.transform.position;
				if (vector.x > 0f && Mathf.Abs(base.transform.position.x - borderRight.position.x) > 0.5f)
				{
					base.transform.Translate(new Vector3(vector.normalized.x * speed * Time.deltaTime, 0f, 0f), Space.World);
					animator.SetBool("Walk", value: true);
				}
				else if (vector.x < 0f && Mathf.Abs(base.transform.position.x - borderLeft.position.x) > 0.5f)
				{
					base.transform.Translate(new Vector3(vector.normalized.x * speed * Time.deltaTime, 0f, 0f), Space.World);
					animator.SetBool("Walk", value: true);
				}
				else
				{
					animator.SetBool("Walk", value: false);
				}
			}
		}
	}

	private void OnCollisionStay2D(Collision2D collision) // If ennemy is colliding with the player
	{
		if (collision.transform.CompareTag("Player") && !ennemyHealth.isShielding && !isKing && !inside)
		{
			isAttacking = true;
			animator.SetTrigger("Attack");
			inside = true;
			StartCoroutine(WaitForAttack());
		}
	}

	private void FindNearestBorder() // Find the nearest border between the left and the right border
	{
		if (Mathf.Abs(base.transform.position.x - borderLeft.position.x) < Mathf.Abs(base.transform.position.x - borderRight.position.x))
		{
			borderNear = borderLeft;
		}
		else
		{
			borderNear = borderRight;
		}
	}

	private void FlipEnnemy(bool _flip) // Flip the ennemy sprite
	{
		if (base.transform.position.x - target.position.x < 0f)
		{
			graphics.flipX = _flip;
		}
		else
		{
			graphics.flipX = !_flip;
		}
	}

	private void OnCollisionExit2D(Collision2D collision) 
	{
		if (collision.transform.CompareTag("Player"))
		{
			inside = false;
		}
	}

	private IEnumerator WaitForAttack() // Wait for the attack animation to finish
	{
		speed = 0f;
		yield return new WaitForSeconds(0.5f);
		if (inside)
		{
			PlayerHealth.instance.TakeDamage(damageOnCollision);
		}
		speed = vitesse;
		inside = false;
		isAttacking = false;
	}
}
