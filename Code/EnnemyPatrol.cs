using System.Collections;
using UnityEngine;

public class EnnemyPatrol : MonoBehaviour
{
	public float speed;

	public Transform[] waypoints;

	public Rigidbody2D rb;

	public int damageOnCollision = 20;

	public SpriteRenderer graphics;

	private Transform target;

	private int destPoint;

	public EnnemyHealth ennemyHealth;

	private void Start()
	{
		target = waypoints[0];
	}

	private void Update()
	{
		if (ennemyHealth.isDead)
		{
			StartCoroutine(SnakeDie());
		}
		Vector3 vector = target.position - base.transform.position;
		base.transform.Translate(vector.normalized * speed * Time.deltaTime, Space.World);
		if (Vector3.Distance(base.transform.position, target.position) < 0.3f) // change direction when reaching waypoint
		{
			destPoint = (destPoint + 1) % waypoints.Length;
			target = waypoints[destPoint];
			graphics.flipX = !graphics.flipX;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) // damage player on collision
	{
		if (collision.transform.CompareTag("Player"))
		{
			collision.transform.GetComponent<PlayerHealth>().TakeDamage(damageOnCollision);
		}
	}

	private IEnumerator SnakeDie() // destroy snake after 0.5s
	{
		yield return new WaitForSeconds(0.5f);
		Object.Destroy(base.gameObject);
	}
}
