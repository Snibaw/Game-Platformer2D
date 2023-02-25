using System.Collections;
using UnityEngine;

public class EnnemyShield : MonoBehaviour
{
	private PlayerFollow playerFollow;

	private EnnemyHealth ennemyHealth;

	private GameObject player;

	private Animator animator;

	private bool processing;

	public float maxDistance;

	public float minDistance;

	public float ShieldTime;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		playerFollow = base.gameObject.GetComponent<PlayerFollow>();
		ennemyHealth = base.gameObject.GetComponent<EnnemyHealth>();
		animator = base.gameObject.GetComponent<Animator>();
	}

	private void Update()
	{
		if (!ennemyHealth.isDead && !processing && !playerFollow.isAttacking)
		{
			StartCoroutine(Shielding());
		}
	}

	private IEnumerator Shielding() // IF the ennemy is not dead, not processing and not attacking, it will shield
	{
		processing = true;
		if (Vector2.Distance(player.transform.position, base.transform.position) < maxDistance && Vector2.Distance(player.transform.position, base.transform.position) > minDistance) // IF the player is in the range of the ennemy, it will shield
		{
			// Shielding
			ennemyHealth.isShielding = true;
			animator.SetBool("Shield", ennemyHealth.isShielding);
			float tempoSpeed = playerFollow.speed;
			playerFollow.speed = tempoSpeed / 8f;
			yield return new WaitForSeconds(ShieldTime + 0.5f);
			ennemyHealth.isShielding = false;
			animator.SetBool("Shield", ennemyHealth.isShielding);
			playerFollow.speed = tempoSpeed;
			yield return new WaitForSeconds(ShieldTime * 4f);
		}
		processing = false;
	}
}
