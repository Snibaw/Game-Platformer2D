using System.Collections;
using UnityEngine;

public class WizardBlueBehaviour : MonoBehaviour
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

	private bool canTp = true;

	private bool isTping;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		ennemyHealth = base.gameObject.GetComponent<EnnemyHealth>();
		timeBtwShots = startTimeBtwShots;
		vitesse = speed;
	}

	private void Update()
	{
		if (ennemyHealth.isDead || isShooting || isTping)
		{
			return;
		}
		if (Vector2.Distance(base.transform.position, player.position) > stoppingDistance) // If the ennemy is too far from the player
		{
			AnimationWalk(); // Play walk animation and flip sprite
			FlipShooter(_flip: false);
			Vector3 vector = player.position - base.transform.position;
			if (vector.x > 0f && Vector2.Distance(base.transform.position, borderRight.position) > 0.5f) // If the player is on the right of the ennemy and the ennemy is not too close to the right border
			{
				base.transform.Translate(new Vector3(vector.normalized.x * vitesse * Time.deltaTime, 0f, 0f), Space.World);
			}
			else if (vector.x < 0f && Vector2.Distance(base.transform.position, borderLeft.position) > 0.5f) // If the player is on the left of the ennemy and the ennemy is not too close to the left border
			{
				base.transform.Translate(new Vector3(vector.normalized.x * vitesse * Time.deltaTime, 0f, 0f), Space.World);
			}
		}
		else if (Vector2.Distance(base.transform.position, player.position) < stoppingDistance && Vector2.Distance(base.transform.position, player.position) > retreatDistance) // If the ennemy is too close to the player
		{
			animator.SetBool("Walk", value: false);
			FlipShooter(_flip: false);
			base.transform.position = base.transform.position;
		}
		else if (Vector2.Distance(base.transform.position, player.position) < retreatDistance) // If the ennemy is too close to the player
		{
			if (canTp)
			{
				StartCoroutine(WizardTP());
			}
			else
			{
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
			AnimationWalk();
		}
		if (timeBtwShots <= 0f && Vector2.Distance(base.transform.position, player.position) < shootingDistance) // If the ennemy can shoot and is close enough to the player
		{
			StartCoroutine(WaitForShot());
		}
		else
		{
			timeBtwShots -= Time.deltaTime;
		}
	}

	private IEnumerator WizardTP() // Teleportation
	{
		isTping = true;
		animator.SetTrigger("Attack1");
		yield return new WaitForSeconds(0.5f);
		if (Mathf.Abs(player.position.x - borderLeft.position.x) < Mathf.Abs(player.position.x - borderRight.position.x))
		{
			base.transform.position = new Vector3(0.5f * (player.position.x + borderRight.position.x), base.transform.position.y, base.transform.position.z);
		}
		else
		{
			base.transform.position = new Vector3(0.5f * (player.position.x + borderLeft.position.x), base.transform.position.y, base.transform.position.z);
		}
		canTp = false;
		isTping = false;
		yield return new WaitForSeconds(3f);
		canTp = true;
	}

	private void FlipShooter(bool _flip) // Flip the sprite
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

	private void AnimationWalk() // Play walk animation
	{
		if (Vector2.Distance(base.transform.position, borderRight.position) < 0.5f || Vector2.Distance(base.transform.position, borderLeft.position) < 0.5f)
		{
			animator.SetBool("Walk", value: false);
		}
		else
		{
			animator.SetBool("Walk", value: true);
		}
	}

	private IEnumerator WaitForShot() // Wait for the shot
	{
		isShooting = true;
		FlipShooter(_flip: false);
		vitesse = 0f;
		animator.SetTrigger("Attack2");
		yield return new WaitForSeconds(0.5f);
		FlipShooter(_flip: false);
		Object.Instantiate(projectile, base.transform.position, Quaternion.identity);
		yield return new WaitForSeconds(0.3f);
		timeBtwShots = startTimeBtwShots;
		vitesse = speed;
		isShooting = false;
	}
}
