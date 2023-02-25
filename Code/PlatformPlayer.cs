using UnityEngine;

public class PlatformPlayer : MonoBehaviour
{
	public Rigidbody2D rb;

	public GameObject player;

	private void Start()
	{
		rb = GetComponentInParent<Rigidbody2D>();
	}

	private void OnTriggerStay2D(Collider2D collision) // If the player is on the platform
	{
		if (collision.gameObject.CompareTag("Platforme"))
		{
			PlayerMovement.instance.isGrounded = true;
			if (Mathf.Abs(rb.velocity.x) < 5f)
			{
				player.transform.parent = collision.gameObject.transform;
			}
			else
			{
				player.transform.parent = null;
			}
			Debug.Log("Sur la Platforme");
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		PlayerMovement.instance.isGrounded = false;
		player.transform.parent = null;
	}
}
