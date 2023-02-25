using UnityEngine;

public class EnnemyHealth : MonoBehaviour
{
	public int maxHealth;

	public int currentHealth;

	private Rigidbody2D rb;

	public GameObject bloodEffect;

	private Animator animator;

	private BoxCollider2D ennemyCollider;

	public bool isDead;

	public bool isShielding;

	public float animationDeathTime;

	public int moneyEarnWhenKilled;

	public float gravityScaleWhenDie;

	private bool hitExist;

	public bool canDodge;

	public bool isDodging;

	public HealthBarBehaviour healthBar;

	public GameObject floatingPoints;

	private void Start()
	{
		Physics2D.IgnoreLayerCollision(8, 8, ignore: true); // Ignore collision between ennemies
		animator = base.gameObject.GetComponent<Animator>();
		rb = base.gameObject.GetComponent<Rigidbody2D>();
		ennemyCollider = base.gameObject.GetComponent<BoxCollider2D>();
		AnimatorControllerParameter[] parameters = animator.parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].name == "Hit")
			{
				hitExist = true;
			}
		}
		currentHealth = maxHealth;
		healthBar.SetHealth(currentHealth, maxHealth);
	}

	private void Update()
	{
		if (currentHealth <= 0 && !isDead)
		{
			WaitForDeath();
		}
	}

	public void TakeDamage(int damage, GameObject effect)
	{
		if (!isShielding && !canDodge)
		{
			if (hitExist)
			{
				animator.SetTrigger("Hit");
			}
			if (effect != null)
			{
				Object.Instantiate(effect, base.transform.position, Quaternion.identity);
			}
			// Print damage on ennemy
			Object.Instantiate(floatingPoints, base.transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
			currentHealth -= damage;
			healthBar.SetHealth(currentHealth, maxHealth);
			Debug.Log("damage Taken from ennemy");
		}
		if (canDodge)
		{
			Object.Instantiate(floatingPoints, base.transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<TextMesh>().text = "0";
			isDodging = true;
		}
		if (isShielding)
		{
			Object.Instantiate(floatingPoints, base.transform.position, Quaternion.identity).transform.GetChild(0).GetComponent<TextMesh>().text = "0";
		}
	}

	private void WaitForDeath()
	{
		// Play death animation, add money to player, destroy ennemy
		animator.SetTrigger("Die");
		CurrentSceneManager.instance.coinsPickedUpInThisSceneCount += moneyEarnWhenKilled;
		Inventory.instance.AddCoins(moneyEarnWhenKilled);
		rb.velocity = Vector3.zero;
		rb.gravityScale = gravityScaleWhenDie;
		rb.bodyType = RigidbodyType2D.Kinematic;
		ennemyCollider.enabled = false;
		isDead = true;
		Object.Destroy(base.gameObject, animationDeathTime);
	}
}
