using System.Collections;
using UnityEngine;

public class GolemBehaviour : MonoBehaviour
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

	public int healPower;

	public bool isHealing;

	public float healingRecoil;

	private float healingTimer;

	private bool inside;

	public float dirx;

	private float vitesse;

	private Rigidbody2D rb;

	private void Start()
	{
		rb = base.gameObject.GetComponent<Rigidbody2D>();
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		vitesse = speed;
	}

	private void Update()
	{
		healingTimer -= Time.deltaTime;
		rb.velocity = Vector3.zero;
		if (PlayerHealth.instance.currentHealth <= 0f)
		{
			animator.SetBool("isWalking", value: false);
		}
		else if (Vector2.Distance(base.transform.position, target.position) < VisionDistance && !isHealing && !ennemyHealth.isDead) // if the player is in the vision distance and the golem is not healing or dead
		{
			FlipEnnemy(_flip: false);
			Vector3 vector = target.position - base.transform.position;
			if (vector.x > 0f && Mathf.Abs(base.transform.position.x - borderRight.position.x) > 0.5f) // If the player is on the right and the golem is not at the right border
			{
				base.transform.Translate(new Vector3(vector.normalized.x * speed * Time.deltaTime, 0f, 0f), Space.World);
				animator.SetBool("isWalking", value: true);
			}
			else if (vector.x < 0f && Mathf.Abs(base.transform.position.x - borderLeft.position.x) > 0.5f) // If the player is on the left and the golem is not at the left border
			{
				base.transform.Translate(new Vector3(vector.normalized.x * speed * Time.deltaTime, 0f, 0f), Space.World);
				animator.SetBool("isWalking", value: true);
			}
			else
			{
				animator.SetBool("isWalking", value: false);
			}
		}
		if (ennemyHealth.currentHealth < ennemyHealth.maxHealth / 2)
		{
			StartCoroutine(Healing(healPower));
		}
	}

	private void OnCollisionStay2D(Collision2D collision) // If the player is in the golem's hitbox
	{
		if (collision.transform.CompareTag("Player") && !inside)
		{
			animator.SetTrigger("Attack");
			inside = true;
			StartCoroutine(WaitForAttack());
		}
	}

	private IEnumerator WaitForAttack() // Wait for the attack animation to finish before dealing damage
	{
		speed = 0f;
		yield return new WaitForSeconds(0.5f);
		if (inside)
		{
			PlayerHealth.instance.TakeDamage(damageOnCollision);
			inside = false;
		}
		speed = vitesse;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.transform.CompareTag("Player"))
		{
			inside = false;
		}
	}

	private IEnumerator Healing(int _heal) // Heal the golem
	{
		if (!isHealing && !ennemyHealth.isDead && healingTimer <= 0f)
		{
			speed = 0f;
			isHealing = true;
			animator.SetTrigger("Heal");
			yield return new WaitForSeconds(0.5f);
			ennemyHealth.TakeDamage(-_heal, null);
			isHealing = false;
			healingTimer = healingRecoil;
			speed = vitesse;
		}
	}

	private void FlipEnnemy(bool _flip) // Flip the sprite of the golem
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
