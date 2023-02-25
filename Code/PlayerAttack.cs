using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	public float timeBtwAttack;

	public float startTimeBtwAttack;

	public Transform attackPos;

	public float attackRangeX;

	public float attackRangeY;

	public LayerMask whatIsEnnemies;

	public int damage;

	public bool isSwordOut;

	public GameObject bloodEffect;

	private float tempoJForce;

	public static PlayerAttack instance;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instancede PlayerAttack dans la scène");
		}
		else
		{
			instance = this;
		}
	}

	private void Update()
	{
		if (timeBtwAttack <= 0f && PlayerHealth.instance.currentHealth > 0f && (Input.GetKey(KeyCode.F) || Input.GetMouseButtonDown(1))) // If the player is not dead, can attack and press F or right click
		{
			timeBtwAttack = startTimeBtwAttack;
			if (!isSwordOut) // If the animation is not the one with the sword out
			{
				PlayerMovement.instance.animator.SetTrigger("showSword");
				isSwordOut = true;
				PlayerMovement.instance.animator.SetBool("isSwordOut", isSwordOut);
			}
			else // The animation is the one with the sword out
			{
				PlayerMovement.instance.animator.SetTrigger("Attack1");
				Debug.Log("Attack1");
			}
			Collider2D[] array = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0f, whatIsEnnemies);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponent<EnnemyHealth>().TakeDamage(damage, bloodEffect);
			}
			StartCoroutine(slowAttack());
		}
		if (timeBtwAttack <= -5f && isSwordOut && PlayerHealth.instance.currentHealth > 0f) // IF the player didn't attack for a long time, hide the sword
		{
			isSwordOut = false;
			PlayerMovement.instance.animator.SetBool("isSwordOut", isSwordOut);
			if (!PlayerMovement.instance.isJumping && !PlayerMovement.instance.isClimbing)
			{
				PlayerMovement.instance.animator.SetTrigger("hideSword");
			}
		}
		timeBtwAttack -= Time.deltaTime;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1f));
	}

	public IEnumerator slowAttack() // Slow the player when he attacks
	{
		tempoJForce = PlayerMovement.instance.jumpForce;
		PlayerMovement.instance.jumpForce = 0f;
		PlayerMovement.instance.moveSpeed = 0f;
		yield return new WaitForSeconds(0.2f);
		PlayerMovement.instance.jumpForce = tempoJForce;
		PlayerMovement.instance.moveSpeed = PlayerMovement.instance.vitesse;
	}
}
