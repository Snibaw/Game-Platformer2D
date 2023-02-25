using System.Collections;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
	private Animator fadeSystem;

	private void Awake()
	{
		fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			StartCoroutine(ReplacePlayer(collision));
		}
	}

	public IEnumerator ReplacePlayer(Collider2D collision)
	{
		// Replace the player at the last checkpoint
		fadeSystem.SetTrigger("FadeIn");
		PlayerMovement.instance.isTalking = true;
		yield return new WaitForSeconds(0.5f);
		PlayerMovement.instance.rb.velocity = Vector3.zero;
		collision.transform.position = CurrentSceneManager.instance.respawnPoint;
		yield return new WaitForSeconds(1f);
		PlayerMovement.instance.isTalking = false;
	}
}
