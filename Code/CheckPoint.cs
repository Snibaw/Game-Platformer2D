using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			CurrentSceneManager.instance.respawnPoint = base.transform.position;
			base.gameObject.GetComponent<BoxCollider2D>().enabled = false;
			base.gameObject.GetComponentInChildren<Animator>().enabled = true;
		}
	}
}
