using System.Collections;
using UnityEngine;

public class EnnemyShooter : MonoBehaviour
{
	public float speed;

	public float stoppingDistance;

	public float retreatDistance;

	public Transform borderLeft;

	public Transform borderRight;

	private float timeBtwShots;

	public float startTimeBtwShots;

	public GameObject projectile;

	public Transform player;

	public float shootingDistance;

	public Animator animator;

	public SpriteRenderer graphics;

	private float vitesse;

	private bool isShooting;

	private EnnemyHealth ennemyHealth;

	public float outOfBorder;

	private bool borderOut;

	private Transform borderNear;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		ennemyHealth = base.gameObject.GetComponent<EnnemyHealth>();
		timeBtwShots = startTimeBtwShots;
		vitesse = speed;
	}

	private void Update()
	{
		outOfBorder -= Time.deltaTime;
		if (ennemyHealth.isDead || isShooting)
		{
			return;
		}
		if (outOfBorder < 0f && borderOut) // If the ennemy is out of the border for more than 0.5 seconds
		{
			FIndNearestBorder();
			base.transform.position = new Vector3(borderNear.position.x, borderLeft.position.y, borderLeft.position.z);
		}
		if (base.transform.position.x < borderLeft.position.x) // If the ennemy is out of the left border
		{
			if (!borderOut)
			{
				borderOut = true;
				outOfBorder = 0.5f;
			}
			base.transform.Translate(new Vector3(1f * speed * Time.deltaTime, 0f, 0f), Space.World);
			animator.SetBool("walk", value: true);
			graphics.flipX = false;
			return;
		}
		if (base.transform.position.x > borderRight.position.x) // If the ennemy is out of the right border
		{
			if (!borderOut)
			{
				borderOut = true;
				outOfBorder = 5f;
			}
			base.transform.Translate(new Vector3(-1f * speed * Time.deltaTime, 0f, 0f), Space.World);
			animator.SetBool("walk", value: true);
			graphics.flipX = true;
			return;
		}
		borderOut = false;
		if (Vector2.Distance(base.transform.position, player.position) > stoppingDistance) // If the ennemy is too far from the player
		{
			AnimationWalk();
			FlipShooter(_flip: false);
			Vector3 vector = player.position - base.transform.position;
			if (vector.x > 0f && Vector2.Distance(base.transform.position, borderRight.position) > 0.5f)
			{
				base.transform.Translate(new Vector3(vector.normalized.x * vitesse * Time.deltaTime, 0f, 0f), Space.World);
			}
			else if (vector.x < 0f && Vector2.Distance(base.transform.position, borderLeft.position) > 0.5f)
			{
				base.transform.Translate(new Vector3(vector.normalized.x * vitesse * Time.deltaTime, 0f, 0f), Space.World);
			}
		}
		else if (Vector2.Distance(base.transform.position, player.position) < stoppingDistance && Vector2.Distance(base.transform.position, player.position) > retreatDistance) // If the ennemy is close enough to the player
		{
			animator.SetBool("walk", value: false);
			FlipShooter(_flip: false);
			base.transform.position = base.transform.position;
		}
		else if (Vector2.Distance(base.transform.position, player.position) < retreatDistance) // If the ennemy is too close to the player
		{
			AnimationWalk();
			Vector3 vector2 = player.position - base.transform.position;
			if (vector2.x > 0f && Vector2.Distance(base.transform.position, borderLeft.position) > 0.5f)
			{
				base.transform.Translate(new Vector3(vector2.normalized.x * (0f - vitesse) * Time.deltaTime, 0f, 0f), Space.World);
				FlipShooter(_flip: true);
			}
			else if (vector2.x < 0f && Vector2.Distance(base.transform.position, borderRight.position) > 0.5f)
			{
				base.transform.Translate(new Vector3(vector2.normalized.x * (0f - vitesse) * Time.deltaTime, 0f, 0f), Space.World);
				FlipShooter(_flip: true);
			}
			else
			{
				FlipShooter(_flip: false);
			}
		}
		if (timeBtwShots <= 0f && Vector2.Distance(base.transform.position, player.position) < shootingDistance)
		{
			StartCoroutine(WaitForShot());
		}
		else
		{
			timeBtwShots -= Time.deltaTime;
		}
	}

	private void FIndNearestBorder() // Find the nearest border between the left and the right border
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

	private void FlipShooter(bool _flip) // Flip the sprite of the ennemy
	{
		if (base.transform.position.x - player.position.x < 0f)
		{
			graphics.flipX = _flip;
		}
		else
		{
			graphics.flipX = !_flip;
		}
	}

	private void AnimationWalk() // Play the walk animation
	{
		if (Vector2.Distance(base.transform.position, borderRight.position) < 0.5f || Vector2.Distance(base.transform.position, borderLeft.position) < 0.5f)
		{
			animator.SetBool("walk", value: false);
		}
		else
		{
			animator.SetBool("walk", value: true);
		}
	}

	private IEnumerator WaitForShot() // Wait for the animation to finish before shooting
	{
		isShooting = true;
		FlipShooter(_flip: false);
		vitesse = 0f;
		animator.SetTrigger("Shot");
		timeBtwShots = startTimeBtwShots;
		yield return new WaitForSeconds(0.75f);
		FlipShooter(_flip: false);
		Object.Instantiate(projectile, base.transform.position, Quaternion.identity);
		yield return new WaitForSeconds(0.4f);
		FlipShooter(_flip: false);
		vitesse = speed;
		isShooting = false;
	}
}
