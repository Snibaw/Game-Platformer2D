using System.Collections;
using UnityEngine;

public class KingBehaviour : MonoBehaviour
{
	public float timeBtwAttack;

	public float startTimeBtwAttack;

	public Transform attackPos;

	public float attackRangeX;

	public float attackRangeY;

	public LayerMask playerLayer;

	private Animator animator;

	private PlayerFollow playerFollow;

	public int damageCombo;

	public int damageSlam;

	public bool lastIsCombo;

	private int damage;

	private EnnemyHealth ennemyHealth;

	private float Offset;

	private bool playerBTWBorders;

	private void Start()
	{
		// Initialisation
		ennemyHealth = base.gameObject.GetComponent<EnnemyHealth>();
		animator = base.gameObject.GetComponent<Animator>();
		playerFollow = base.gameObject.GetComponent<PlayerFollow>();
		playerFollow.isKing = true;
		timeBtwAttack = startTimeBtwAttack;
		ennemyHealth.canDodge = true;
	}

	private void Update()
	{
		if (ennemyHealth.isDead)
		{
			return;
		}
		timeBtwAttack -= Time.deltaTime;
		if (timeBtwAttack <= 0f && !ennemyHealth.isDodging) // If the ennemy can attack
		{
			attackPos.transform.position = base.transform.position;
			Collider2D[] array = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0f, playerLayer); // Search for the player in the attack range
			for (int i = 0; i < array.Length; i++)
			{
				if (lastIsCombo)
				{
					animator.SetTrigger("Ground");
					damage = damageSlam;
				}
				else
				{
					animator.SetTrigger("Combo");
					damage = damageCombo;
				}
				StartCoroutine(Attack(array[i], damage));
			}
		}
		if (ennemyHealth.isDodging && playerFollow.speed != 0f) // If the ennemy is dodging
		{
			ennemyHealth.isDodging = false;
			StartCoroutine(Dodge());
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1f));
	}

	private IEnumerator Attack(Collider2D _playerCollider, int _damage)
	{
		// Attacking the player
		timeBtwAttack = startTimeBtwAttack;
		playerFollow.speed = 0f;
		yield return new WaitForSeconds(0.7f);
		DamagePlayer(_playerCollider, _damage);
		if (_damage == damageCombo)
		{
			yield return new WaitForSeconds(0.3f);
			DamagePlayer(_playerCollider, _damage);
		}
		playerFollow.speed = playerFollow.vitesse;
		timeBtwAttack = startTimeBtwAttack;
		lastIsCombo = !lastIsCombo;
	}

	private void DamagePlayer(Collider2D _playerCollider, int _damage)
	{
		// Doing damage to the player
		Collider2D[] array = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0f, playerLayer);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == _playerCollider)
			{
				_playerCollider.GetComponent<PlayerHealth>().TakeDamage(_damage);
			}
		}
	}

	private IEnumerator Dodge()
	{
		// Dodging the player
		playerFollow.speed = 0f;
		animator.SetTrigger("Teleport");
		yield return new WaitForSeconds(1f);
		PlayerBTWBorder();
		if (playerBTWBorders)
		{
			if (base.transform.position.x - playerFollow.target.position.x > 0f)
			{
				Offset = -1f;
			}
			else
			{
				Offset = 1f;
			}
			base.transform.position = new Vector3(playerFollow.target.position.x + Offset, base.transform.position.y, base.transform.position.z);
		}
		playerFollow.speed = playerFollow.vitesse;
		ennemyHealth.canDodge = false;
		yield return new WaitForSeconds(5f);
		ennemyHealth.canDodge = true;
	}

	private void PlayerBTWBorder()
	{
		// Check if the player is between the borders
		if (playerFollow.target.transform.position.x - playerFollow.borderLeft.position.x > 0f && playerFollow.target.transform.position.x - playerFollow.borderRight.position.x < 0f)
		{
			playerBTWBorders = true;
		}
		else
		{
			playerBTWBorders = false;
		}
	}
}
