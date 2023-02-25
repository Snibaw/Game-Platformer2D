using System.Collections;
using UnityEngine;

public class EnnemyFly : MonoBehaviour
{
	public float speed;

	public float stoppingDistance;

	private Transform player;

	public Animator animator;

	public SpriteRenderer graphics;

	private float vitesse;

	private EnnemyHealth ennemyHealth;

	public Transform[] waypoints;

	private Transform target;

	private int destPoint;

	private Rigidbody2D rb;

	private bool inside;

	public int damageOnCollision;

	private bool followPlayer;

	private bool isAttacking;

	public float TimeBtwAttack;

	public float TimeBtwAttackTimer;

	public float outOfBorder;

	private bool borderOut;

	private Transform borderNear;

	private void Start()
	{
		rb = base.gameObject.GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		ennemyHealth = base.gameObject.GetComponent<EnnemyHealth>();
		vitesse = speed;
		target = waypoints[0];
		TimeBtwAttack = TimeBtwAttackTimer;
	}

	private void Update()
	{
		outOfBorder -= Time.deltaTime;
		TimeBtwAttack -= Time.deltaTime;
		rb.velocity = Vector3.zero;
		if (ennemyHealth.isDead)
		{
			return;
		}
		if (target.position.x - base.transform.position.x < 0f)
		{
			graphics.flipX = true;
		}
		else
		{
			graphics.flipX = false;
		}
		if (Vector2.Distance(base.transform.position, player.position) > stoppingDistance && !followPlayer) // If the distance between the player and the enemy is greater than the stopping distance, the enemy will move towards the player.
		{
			target = waypoints[destPoint];
			Vector3 vector = target.position - base.transform.position;
			base.transform.Translate(vector.normalized * speed * Time.deltaTime, Space.World);
			if (Vector3.Distance(base.transform.position, target.position) < 0.3f)
			{
				destPoint = (destPoint + 1) % waypoints.Length;
			}
			return;
		}
		if (outOfBorder < 0f && borderOut) // If the enemy is out of the border, it will go back to the border.
		{
			FIndNearestBorder();
			base.transform.position = new Vector3(borderNear.position.x, waypoints[0].position.y, waypoints[0].position.z);
		}
		if (base.transform.position.x < waypoints[0].position.x) // If the enemy is in the border, it will follow the player.
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
		else if (base.transform.position.x > waypoints[1].position.x) // If the enemy is in the border, it will follow the player.
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
		else
		{
			borderOut = false;
			followPlayer = true;
			target = player;
			Vector3 vector2 = target.position - base.transform.position;
			base.transform.Translate(vector2.normalized * vitesse * Time.deltaTime, Space.World);
		}
	}

	private void FIndNearestBorder() // Find the nearest weapoint between the two waypoints
	{
		if (Mathf.Abs(base.transform.position.x - waypoints[0].position.x) < Mathf.Abs(base.transform.position.x - waypoints[1].position.x))
		{
			borderNear = waypoints[0];
		}
		else
		{
			borderNear = waypoints[1];
		}
	}

	private void OnCollisionStay2D(Collision2D collision) // If the enemy is in the player, it will attack him.
	{
		if (collision.transform.CompareTag("Player") && !inside && TimeBtwAttack <= 0f && !isAttacking)
		{
			animator.SetTrigger("Attack");
			inside = true;
			StartCoroutine(WaitForAttack());
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("Player"))
		{
			inside = false;
		}
	}

	private IEnumerator WaitForAttack() // Wait for the animation to finish before damaging the player.
	{
		if (!isAttacking)
		{
			isAttacking = true;
			rb.velocity = Vector3.zero;
			vitesse = 0f;
			yield return new WaitForSeconds(0.5f);
			if (inside)
			{
				PlayerHealth.instance.TakeDamage(damageOnCollision);
				inside = false;
			}
			vitesse = speed * 2f;
			yield return new WaitForSeconds(0.15f);
			vitesse = speed;
			isAttacking = false;
			inside = false;
			TimeBtwAttack = TimeBtwAttackTimer;
		}
	}
}
