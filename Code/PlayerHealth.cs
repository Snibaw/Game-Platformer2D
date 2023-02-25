using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	public float maxHealth = 100f;

	public float currentHealth;

	public AudioClip hitSound;

	public bool isInvincible;

	public Animator animator;

	public SpriteRenderer graphics;

	public HealthBar healthBar;

	public float regen;

	public static PlayerHealth instance;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("Il y a plus d'une instancede PlayerHealth dans la scène");
		}
		else
		{
			instance = this;
		}
	}

	private void Start()
	{
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
	}

	private void Update()
	{
		HealPlayer(regen);
	}

	public void HealPlayer(float amount)
	{
		if (currentHealth + amount > maxHealth)
		{
			currentHealth = maxHealth;
		}
		else
		{
			currentHealth += amount;
		}
		healthBar.SetHealth(currentHealth);
	}

	public void TakeDamage(int damage)
	{
		if (!isInvincible)
		{
			AudioManager.instance.PlayClipAt(hitSound, base.transform.position);
			currentHealth -= damage;
			healthBar.SetHealth(currentHealth);
			if (currentHealth <= 0f)
			{
				Die();
				return;
			}
			isInvincible = true;
			StartCoroutine(InvicibilityFlash());
			StartCoroutine(HandleInvincibilityDelay());
		}
	}

	public void Die()
	{
		// Desactivate movement and animation
		StartCoroutine(RedDeath());
		PlayerMovement.instance.enabled = false;
		PlayerMovement.instance.animator.SetTrigger("Die");
		PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Kinematic;
		PlayerMovement.instance.rb.velocity = Vector3.zero;
		PlayerMovement.instance.playerCollider.enabled = false;
		GameOverManager.instance.OnPlayerDeath();
	}

	public void Respawn()
	{
		// Activate movement and animation
		PlayerMovement.instance.enabled = true;
		PlayerMovement.instance.animator.SetTrigger("Respawn");
		PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Dynamic;
		PlayerMovement.instance.playerCollider.enabled = true;
		currentHealth = maxHealth;
		healthBar.SetHealth(currentHealth);
	}

	public IEnumerator InvicibilityFlash()
	{
		// Animation when player is invincible
		while (isInvincible)
		{
			graphics.color = new Color(1f, 1f, 1f, 0f);
			yield return new WaitForSeconds(0.15f);
			graphics.color = new Color(1f, 1f, 1f, 1f);
			yield return new WaitForSeconds(0.15f);
		}
	}

	public IEnumerator HandleInvincibilityDelay()
	{
		yield return new WaitForSeconds(1.5f);
		isInvincible = false;
	}

	public IEnumerator RedDeath()
	{
		// Animation when player die
		graphics.color = new Color(1f, 0f, 0f, 1f);
		yield return new WaitForSeconds(0.15f);
		graphics.color = new Color(1f, 1f, 1f, 1f);
	}
}
