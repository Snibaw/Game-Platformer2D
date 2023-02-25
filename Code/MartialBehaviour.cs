using System.Collections;
using UnityEngine;

public class MartialBehaviour : MonoBehaviour
{
	public EnnemyHealth ennemyHealth;

	private Transform target;

	public float VisionDistance;

	public SpriteRenderer graphics;

	public float speed;

	public Transform borderLeft;

	public Transform borderRight;

	public Animator animator;

	public int damageOnCollision;

	private bool inside;

	private Rigidbody2D rb;

	public float timingAttack;

	public float timingAttackTImer;

	private float vitesse;

	private bool dontPush;

	private void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		rb = base.gameObject.GetComponent<Rigidbody2D>();
		vitesse = speed;
	}

	private void Update()
	{
		timingAttack -= Time.deltaTime;
		rb.velocity = Vector3.zero;
		if (PlayerHealth.instance.currentHealth <= 0f)
		{
			animator.SetBool("Walk", value: false);
		}
		else if (!ennemyHealth.isDead && Vector2.Distance(base.transform.position, target.position) < VisionDistance && !dontPush) // If the player is in the vision distance and the enemy is not dead
		{
			FlipEnnemy(_flip: false);
			Vector3 vector = target.position - base.transform.position;
			if (vector.x > 0f && Mathf.Abs(base.transform.position.x - borderRight.position.x) > 0.5f) // If the player is on the right of the enemy and the enemy is not at the right border
			{
				base.transform.Translate(new Vector3(vector.normalized.x * speed * Time.deltaTime, 0f, 0f), Space.World);
				animator.SetBool("Walk", value: true);
			}
			else if (vector.x < 0f && Mathf.Abs(base.transform.position.x - borderLeft.position.x) > 0.5f) // If the player is on the left of the enemy and the enemy is not at the left border
			{
				base.transform.Translate(new Vector3(vector.normalized.x * speed * Time.deltaTime, 0f, 0f), Space.World);
				animator.SetBool("Walk", value: true);
			}
			else
			{
				animator.SetBool("Walk", value: false);
			}
		}
		else
		{
			animator.SetBool("Walk", value: false);
		}
	}

	private void OnCollisionStay2D(Collision2D collision) // If the player is in the collision box
	{
		if (collision.transform.CompareTag("Player"))
		{
			dontPush = true;
			if (!inside && timingAttack <= 0f)
			{
				animator.SetTrigger("Attack");
				inside = true;
				StartCoroutine(WaitForAttack());
			}
		}
	}

	private IEnumerator WaitForAttack() // Wait for the animation to finish and then deal damage
	{
		speed = 0f;
		yield return new WaitForSeconds(0.6f);
		if (inside)
		{
			PlayerHealth.instance.TakeDamage(damageOnCollision);
		}
		yield return new WaitForSeconds(0.6f);
		if (inside)
		{
			PlayerHealth.instance.TakeDamage(damageOnCollision);
		}
		inside = false;
		timingAttack = timingAttackTImer;
		speed = vitesse;
	}

	private void OnCollisionExit2D(Collision2D collision) // If the player is not in the collision box
	{
		if (collision.transform.CompareTag("Player"))
		{
			inside = false;
			dontPush = false;
		}
	}

	private void FlipEnnemy(bool _flip) // Flip the ennemy sprite depending on the player position
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
}
