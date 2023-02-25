using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
	private Animator animator;

	public BoxCollider2D box;

	private bool isFalling;

	private void Start()
	{
		animator = base.gameObject.GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			StartCoroutine(StartFalling());
		}
	}

	private IEnumerator StartFalling()
	{
		if (!isFalling)
		{
			// Start falling
			isFalling = true;
			animator.SetTrigger("Falling");
			yield return new WaitForSeconds(3.8f);
			box.enabled = false;
			yield return new WaitForSeconds(4.2f);
			box.enabled = true;
			Debug.Log("Platform Tombe");
			isFalling = false;
		}
	}
}
