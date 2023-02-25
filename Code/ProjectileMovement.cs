using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
	public float speed;

	private Transform player;

	private Vector3 target;

	private Vector3 dir;

	public float lifetime;

	public int damage;

	private void Start()
	{
		// Find the player
		player = GameObject.FindGameObjectWithTag("Player").transform;
		target = new Vector3(player.position.x, player.position.y, player.position.z);
		dir = target - base.transform.position;
		if (dir.x > 0f) // If the player is on the right side of the projectile
		{
			base.gameObject.GetComponent<SpriteRenderer>().flipX = true;
		}
		else 
		{
			base.gameObject.GetComponent<SpriteRenderer>().flipX = false;
		}
	}

	private void Update()
	{
		lifetime -= Time.deltaTime;
		base.transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
		if (lifetime <= 0f)
		{
			DestroyProjectile();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) // If the projectile hits something
	{
		if (collision.CompareTag("Player"))
		{
			PlayerHealth.instance.TakeDamage(damage);
			DestroyProjectile();
		}
		if (collision.CompareTag("Bloc (stop projectile)"))
		{
			DestroyProjectile();
		}
	}

	private void DestroyProjectile()
	{
		Object.Destroy(base.gameObject);
	}
}
